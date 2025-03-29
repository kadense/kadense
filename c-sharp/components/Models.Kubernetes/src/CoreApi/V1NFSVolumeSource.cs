namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NFSVolumeSource
    {
        public string? Server { get; set; }
        public string? Path { get; set; }
        public bool? ReadOnly { get; set; }
    }
}