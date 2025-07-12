namespace Kadense.Storage;

public class StoredFile
{
    public long? Size { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    public List<StoredFileVersion> Versions { get; set; } = new List<StoredFileVersion>();
    public StoredFileState State { get; set; } = StoredFileState.Active;
    public enum StoredFileState
    {
        Active,
        PendingDeletion,
        Purged
    }
}