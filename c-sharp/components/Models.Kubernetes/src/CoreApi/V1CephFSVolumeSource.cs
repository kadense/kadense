namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CephFSVolumeSource
    { 
        public List<string>? Monitors { get; set; }
        public string? Path { get; set; }
        public string? User { get; set; }
        public string? SecretFile { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public bool? ReadOnly { get; set; }
    }
}