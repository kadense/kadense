namespace Kadense.Storage;

public class StoredChunk
{
    public int? ChunkIndex { get; set; }
    public List<StoredChunkReplica> Replicas { get; set; } = new List<StoredChunkReplica>();
}