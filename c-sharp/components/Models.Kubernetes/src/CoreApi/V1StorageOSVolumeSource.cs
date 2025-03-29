namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1StorageOSVolumeSource
    {
        public string? VolumeName { get; set; }
        public string? VolumeNamespace { get; set; }
        public bool? ReadOnly { get; set; }
        public string? FsType { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
    }
}