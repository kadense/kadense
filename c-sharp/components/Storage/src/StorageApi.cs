using System.IO.Pipelines;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kadense.Storage;

public class StorageApi
{
    public StorageApi(StorageApiMedia media, StorageApiNodeDistributor nodeDistributor, StorageApiChunkReplicaSelector chunkReplicaSelector, string basePath, int maximumChunkSize, ILoggerProvider? loggingProvider)
    {

        Media = media;
        Media.Api = this; // Set the API reference in the media

        NodeDistributor = nodeDistributor;
        NodeDistributor.Api = this; // Set the API reference in the distributor

        ChunkReplicaSelector = chunkReplicaSelector;
        ChunkReplicaSelector.Api = this; // Set the API reference in the selector

        BasePath = basePath;
        MaximumChunkSize = maximumChunkSize;


        if (loggingProvider != null)
        {
            Logger = loggingProvider.CreateLogger(nameof(StorageApi));
            Media.Logger = loggingProvider.CreateLogger(Media.GetType().Name);
            NodeDistributor.Logger = loggingProvider.CreateLogger(NodeDistributor.GetType().Name);
            ChunkReplicaSelector.Logger = loggingProvider.CreateLogger(ChunkReplicaSelector.GetType().Name);
        }
        else
        {
            Logger = NullLogger.Instance;
        }
    }
    public ILogger Logger { get; }
    public int MaximumChunkSize { get; set; }
    public StorageApiNodeDistributor NodeDistributor { get; }

    public StorageApiChunkReplicaSelector ChunkReplicaSelector { get; }
    public StorageApiMedia Media { get; }

    public int NumberOfReplicas { get; set; } = 2;

    public string BasePath { get; }

    public IEndpointRouteBuilder MapRoutes(IEndpointRouteBuilder endpoints, Action<string,IEndpointConventionBuilder>? endpointConfiguration = null)
    {
        if (endpointConfiguration == null)
            endpointConfiguration = (name, builder) => { };

        endpoints.MapGet($"{BasePath}/files", ListFilesAsync);

        // chunks
        endpoints.MapGet($"{BasePath}/chunks/{{*filePath}}", ReadItemChunkAsync);
        endpoints.MapPost($"{BasePath}/chunks/{{*filePath}}", WriteItemChunkAsync);
        endpoints.MapDelete($"{BasePath}/chunks/{{*filePath}}", DeleteItemChunkAsync);

        // files
        endpoints.MapGet($"{BasePath}/files/{{*filePath}}", ReadFileAsync);
        endpoints.MapPost($"{BasePath}/files/{{*filePath}}", WriteFileAsync);
        endpoints.MapDelete($"{BasePath}/files/{{*filePath}}", DeleteFileAsync);

        // indexes
        endpoints.MapGet($"{BasePath}/index/{{*filePath}}", ReadIndexAsync);
        endpoints.MapPost($"{BasePath}/index/{{*filePath}}", WriteIndexAsync);
        endpoints.MapDelete($"{BasePath}/index/{{*filePath}}", DeleteIndexAsync);
        return endpoints;
    }

    protected virtual async Task ListFilesAsync(HttpContext context)
    {
        var items = await Media.ListItemsAsync();
        await context.Response.WriteAsJsonAsync(items);
    }

    protected virtual async Task ReadIndexAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }
        
        var storedFile = await Media.ReadIndexAsync(filePath);
        await context.Response.WriteAsJsonAsync(storedFile);
        Logger.LogInformation($"Item index for {filePath} retrieved successfully");
    }


    protected virtual async Task ReadFileAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            Logger.LogError($"Missing filePath in request for item {context.Request.Path}");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }

        Logger.LogInformation($"Getting Item for {filePath}");

        var nodeClients = await NodeDistributor.GetNodeOrderAsync();

        var indexTasks = nodeClients.Select(client => client.ReadIndexAsync(filePath)).ToList();
        var successfulIndexes = await indexTasks.WhenSuccessfulAsync();

        if (successfulIndexes.Count() == 0)
        {
            Logger.LogError($"Unable to retrieve file index for {filePath} from any node.");
            context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
            await context.Response.WriteAsync("Failed to retrieve file index from all nodes.");
            return;
        }

        // Order by LastModified descending, pick the latest
        var latestIndex = successfulIndexes
            .Where(x => x != null)
            .OrderByDescending(x => x!.LastModified)
            .First();

        var latestVersion = latestIndex!.Versions.OrderByDescending(v => v.Version).FirstOrDefault();
        if (latestVersion == null || latestVersion.Chunks == null || latestVersion.Chunks.Count == 0)
        {
            Logger.LogError($"No chunks found for the file.");
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableContent;
            await context.Response.WriteAsync("No chunks found for file.");
            return;
        }

        context.Response.StatusCode = 200;

        foreach (var chunk in latestVersion.Chunks.OrderBy(c => c.ChunkIndex))
        {
            // Try each replica in order, use the first that succeeds
            bool chunkFetched = false;
            var replicas = await ChunkReplicaSelector.GetReplicaOrderAsync(chunk);
            foreach (var replica in replicas)
            {
                try
                {
                    var client = nodeClients.First();
                    var chunkBytes = await client.ReadChunkAsync(replica.Url!);
                    await context.Response.Body.WriteAsync(chunkBytes);
                    chunkFetched = true;
                    Logger.LogInformation($"Chunk {chunk.ChunkIndex} fetched successfully from replica {replica.Url}.");
                    break; // Exit the loop on success
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Unable to fetch chunk {ChunkIndex} from replica {ReplicaUrl}.", chunk.ChunkIndex, replica.Url);
                }
            }

            if (!chunkFetched)
            {
                Logger.LogError($"Failed to fetch chunk {chunk.ChunkIndex} from all replicas.");
                context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
                await context.Response.WriteAsync($"Failed to fetch chunk {chunk.ChunkIndex}.");
                return;
            }

            // Flush the response body to ensure the chunk is sent immediately
            await context.Response.Body.FlushAsync();
        }
    }


    protected virtual async Task ReadItemChunkAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }
        
        using (var stream = await Media.ReadItemChunkAsync(filePath))
        {
            context.Response.ContentType = "application/octet-stream";
            context.Response.StatusCode = 200;
            await stream.CopyToAsync(context.Response.Body);
        }
    }

    protected virtual async Task WriteFileAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }

        var nodeClients = await NodeDistributor.GetNodeOrderAsync();

        StoredFile? storedFile = await Media.ReadIndexAsync(filePath);
        int version = 1;

        if (storedFile == null)
        {
            storedFile = new StoredFile
            {
                Size = context.Request.ContentLength,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Versions = new List<StoredFileVersion>
                {
                    new StoredFileVersion
                    {
                        Version = 1
                    }
                }
            };
        }
        else
        {
            version = storedFile.Versions.First().Version + 1;
            storedFile.LastModified = DateTime.UtcNow;
            storedFile.Versions.Insert(0, new StoredFileVersion
            {
                Version = version
            });
        }


        var buffer = new byte[MaximumChunkSize];
        int bytesRead;
        var stream = context.Request.Body;
        int chunkId = 0;
        while ((bytesRead = await stream.ReadAsync(buffer, 0, MaximumChunkSize)) > 0)
        {
            nodeClients = await NodeDistributor.GetNodeOrderAsync();
            var chunk = new StoredChunk
            {
                ChunkIndex = chunkId,
                Replicas = new List<StoredChunkReplica>()
            };
            chunkId++;
            var tasks = new List<Task<string>>();
            for (int replica = 0; replica < NumberOfReplicas; replica++)
            {
                var nodeApi = nodeClients[replica];
                var chunkContent = new ByteArrayContent(buffer, 0, bytesRead);
                var chunkUrl = $"{nodeClients[replica].BaseUrl}/chunks/{filePath}/{version}/{chunkId}";
                tasks.Add(nodeClients[replica].WriteChunkAsync(chunkUrl, chunkContent));

                chunk.Replicas.Add(new StoredChunkReplica
                {
                    Url = chunkUrl,
                });
            }

            var responses = await Task.WhenAll(tasks);
            for (int i = 0; i < responses.Length; i++)
            {
                if (responses[i] == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
                    await context.Response.WriteAsync($"Failed to forward chunk to {nodeClients[i].BaseUrl}");
                    return;
                }
                chunk.Replicas[i].Url = $"{chunk.Replicas[i].Url}/{responses[i]}";
            }
            storedFile.Versions.First().Chunks.Add(chunk);
        }

        List<Task> indexTasks = nodeClients.Select(client => client.WriteIndexAsync(filePath, storedFile)).ToList();
        await Task.WhenAll(indexTasks);
        context.Response.StatusCode = (int)HttpStatusCode.Created;
        await context.Response.WriteAsJsonAsync(storedFile);
    }

    protected virtual async Task WriteItemChunkAsync(HttpContext context)
    {
        // Generate a unique identifier for the chunk

        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }

        var chunkId = Guid.NewGuid().ToString("N");
        var chunkPath = Path.Combine(filePath, chunkId);


        await Media.WriteItemChunkAsync(chunkPath, context.Request.Body);

        // Return the unique identifier
        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(chunkId);
    }

    protected virtual async Task WriteIndexAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }

        var definition = await context.Request.ReadFromJsonAsync<StoredFile>();

        await Media.WriteIndexAsync(filePath, definition!);

        var centralIndex = await Media.ListItemsAsync() ?? [];
    

        centralIndex.Add(filePath);
        var centralIndexDistinct = centralIndex.Distinct().Order().ToList();

        await Media.WriteItemsListAsync(centralIndexDistinct);
    }

    protected virtual Task DeleteFileAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }

    protected virtual async Task DeleteItemChunkAsync(HttpContext context)
    {
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }
        await Media.DeleteItemChunkAsync(filePath);
    }
    protected virtual async Task DeleteIndexAsync(HttpContext context)
    {
 
        var filePath = context.Request.RouteValues["filePath"]?.ToString();
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing filePath.");
            return;
        }
        await Media.DeleteIndexAsync(filePath);
    }
}