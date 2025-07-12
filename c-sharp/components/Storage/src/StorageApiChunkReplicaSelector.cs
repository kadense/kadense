using System.Security.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kadense.Storage;

public abstract class StorageApiChunkReplicaSelector
{
    public ILogger Logger { get; set; } = NullLogger.Instance;
    public StorageApi? Api { get; set; }
    public abstract Task<StoredChunkReplica[]> GetReplicaOrderAsync(StoredChunk chunk);
}

public class StorageApiChunkSelectorRandom : StorageApiChunkReplicaSelector
{
     public override Task<StoredChunkReplica[]> GetReplicaOrderAsync(StoredChunk chunk)
    {
        if (chunk.Replicas.Count == 0)
            throw new InvalidOperationException("No replicas available for the chunk.");

        var chunkArray = chunk.Replicas.ToArray();
        Shuffle(chunkArray);
        return Task.FromResult(chunkArray);
    }

    protected void Shuffle(StoredChunkReplica[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
