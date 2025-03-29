namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ScaleIOVolumeSource
    {
        public string? FsType { get; set; }
        public string? Gateway { get; set; }
        public string? ProtectionDomain { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public bool? SslEnabled { get; set; }
        public string? StorageMode { get; set; }
        public string? StoragePool { get; set; }
        public string? System { get; set; }
        public string? VolumeName { get; set; }
    }
}