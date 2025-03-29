namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CSIVolumeSource
    {
        public string? Driver { get; set; }
        public string? FsType { get; set; }
        public V1LocalObjectReference? NodePublishSecretRef { get; set; }
        public bool? ReadOnly { get; set; }
        public Dictionary<string, string>? VolumeAttributes { get; set; }
    }
}