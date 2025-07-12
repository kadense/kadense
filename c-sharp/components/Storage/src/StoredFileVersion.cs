namespace Kadense.Storage;

public class StoredFileVersion
{
    public int Version { get; set; } = 1;
    public List<StoredChunk> Chunks { get; set; } = new List<StoredChunk>();
}