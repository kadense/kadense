namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GCEPersistentDiskVolumeSource
    {
        public string? PdName { get; set; }
        public string? FsType { get; set; }
        public int? Partition { get; set; }
        public bool? ReadOnly { get; set; }
    }
}