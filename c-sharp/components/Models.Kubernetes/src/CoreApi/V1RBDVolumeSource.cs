namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1RBDVolumeSource
    {
        public string? FsType { get; set; }
        public string? Keyring { get; set; }
        public List<string>? Monitors { get; set; }
        public string? Pool { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public string? User { get; set; }
    }
}