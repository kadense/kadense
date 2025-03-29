namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1FlexVolumeSource
    {
        public string? Driver { get; set; }
        public Dictionary<string, string>? Options { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public string? FsType { get; set; }
    }
}