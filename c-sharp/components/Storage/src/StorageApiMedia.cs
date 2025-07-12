using Microsoft.Extensions.Logging.Abstractions;

namespace Kadense.Storage;

public abstract class StorageApiMedia
{
    public ILogger Logger { get; set; } = NullLogger.Instance;
    public StorageApi? Api { get; set; }
    public abstract Task<List<string>?> ListItemsAsync();
    public abstract Task WriteItemsListAsync(List<string> items);

    public abstract Task<StoredFile?> ReadIndexAsync(string path);
    public abstract Task WriteIndexAsync(string path, StoredFile file);
    public abstract Task DeleteIndexAsync(string path);

    public abstract Task<Stream> ReadItemChunkAsync(string path);
    public abstract Task WriteItemChunkAsync(string path, Stream inboundStream);
    public abstract Task DeleteItemChunkAsync(string path);

}