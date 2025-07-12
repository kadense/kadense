using System.IO.Pipelines;
using System.Net;
using System.Text.Json;

namespace Kadense.Storage;

public class StorageApiFactory
{
    public StorageApiMedia? Media { get; private set; }
    public StorageApiChunkReplicaSelector? ReplicaSelector { get; private set; }
    public StorageApiNodeDistributor? NodeDistributor { get; private set; }
    public ILoggerProvider? LoggerProvider { get; set; }
    public string BasePath { get; private set; } = "/api/storage";
    public int MaximumChunkSize { get; private set; } = 1024; // Default chunk size
    public StorageApiFactory WithMedia(StorageApiMedia media)
    {
        Media = media ?? throw new ArgumentNullException(nameof(media));
        return this;
    }

    public StorageApiFactory WithNodeDistributor(StorageApiNodeDistributor nodeDistributor)
    {
        NodeDistributor = nodeDistributor ?? throw new ArgumentNullException(nameof(nodeDistributor));
        return this;
    }

    public StorageApiFactory WithReplicaSelector(StorageApiChunkReplicaSelector replicaSelector)
    {
        ReplicaSelector = replicaSelector ?? throw new ArgumentNullException(nameof(replicaSelector));
        return this;
    }

    public StorageApiFactory WithLoggerProvider(ILoggerProvider loggerProvider)
    {
        LoggerProvider = loggerProvider;
        return this;
    }

    public StorageApiFactory WithBasePath(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
            throw new ArgumentException("Base path cannot be null or empty.", nameof(basePath));
        BasePath = basePath;
        return this;
    }

    public StorageApiFactory WithMaximumChunkSize(int maximumChunkSize)
    {
        if (maximumChunkSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(maximumChunkSize), "Maximum chunk size must be greater than zero.");
        MaximumChunkSize = maximumChunkSize;
        return this;
    }
    
    public StorageApi Build()
    {
        return new StorageApi(
            Media!,
            NodeDistributor!,
            ReplicaSelector ?? new StorageApiChunkSelectorRandom(),
            BasePath,
            MaximumChunkSize,
            LoggerProvider
        );
    }
}