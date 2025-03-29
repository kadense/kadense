namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GlusterfsVolumeSource
    {
        public string? Endpoints { get; set; }
        public string? Path { get; set; }
        public bool? ReadOnly { get; set; }
    }
}