using System.Text.Json;

namespace Kadense.Storage;

public class FileSystemStorageApiMedia : StorageApiMedia
{
    public FileSystemStorageApiMedia(string basePath)
    {
        BasePath = basePath;
        Directory.CreateDirectory(BasePath);
    }

    public string BasePath { get; }
    public override async Task<StoredFile?> ReadIndexAsync(string path)
    {
        var fullPath = Path.Combine(BasePath, path, "index.json");
        if (!File.Exists(fullPath))
            return null;

        Logger.LogInformation($"Reading index from {fullPath}");
        using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            var storedFile = await JsonSerializer.DeserializeAsync<StoredFile>(fileStream);
            fileStream.Close();
            return storedFile;
        }
    }

    public override async Task WriteIndexAsync(string path, StoredFile file)
    {
        var fullPath = Path.Combine(BasePath, path, "index.json");
        Logger.LogInformation($"Writing index to {fullPath}");
        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await JsonSerializer.SerializeAsync(fileStream, file);
            fileStream.Close();
        }
    }

    public override async Task<List<string>?> ListItemsAsync()
    {
        var fullPath = Path.Combine(BasePath, "index.json");
        if (!File.Exists(fullPath))
            return null;

        Logger.LogInformation($"Reading index from {fullPath}");
        using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            var items = await JsonSerializer.DeserializeAsync<List<string>>(fileStream);
            fileStream.Close();
            return items;
        }
    }

    public override async Task WriteItemsListAsync(List<string> items)
    {
        var fullPath = Path.Combine(BasePath, "index.json");
        Logger.LogInformation($"Writing item list to {fullPath}");
        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await JsonSerializer.SerializeAsync(fileStream, items);
            fileStream.Close();
        }
    }

    public override Task<Stream> ReadItemChunkAsync(string path)
    {
        var fullPath = Path.Combine(BasePath, path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Item chunk not found at {fullPath}");

        Logger.LogInformation($"Reading item chunk at {fullPath}");
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult<Stream>(stream);
    }

    public override async Task WriteItemChunkAsync(string path, Stream inboundStream)
    {
        var fullPath = Path.Combine(BasePath, path);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException("Invalid path for item chunk."));

        Logger.LogInformation($"Writing item chunk at {fullPath}");
        using (var outboundStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await inboundStream.CopyToAsync(outboundStream);
            outboundStream.Close();
        }
    }

    public override Task DeleteIndexAsync(string path)
    {
        var fullPath = Path.Combine(BasePath, path, "index.json");
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Logger.LogInformation($"Deleted index at {fullPath}");
        }
        else
        {
            Logger.LogWarning($"Index file not found at {fullPath}");
        }
        return Task.CompletedTask;
    }

    public override Task DeleteItemChunkAsync(string path)
    {
        var fullPath = Path.Combine(BasePath, path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Logger.LogInformation($"Deleted index at {fullPath}");
        }
        else
        {
            Logger.LogWarning($"Index file not found at {fullPath}");
        }
        return Task.CompletedTask;
    }
}