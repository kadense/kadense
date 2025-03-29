namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1QuobyteVolumeSource
    {
        public string? Registry { get; set; }
        public bool? ReadOnly { get; set; }
        public string? User { get; set; }
        public string? Group { get; set; }
        public string? Tenant { get; set; }
    }
}