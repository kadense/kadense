namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureDiskVolumeSource
    {
        public string? CachingMode { get; set; }
        public string? DiskName { get; set; }
        public string? DiskURI { get; set; }
        public string? FSType { get; set; }
        public int? Kind { get; set; }
        public bool? ReadOnly { get; set; }
    }
}