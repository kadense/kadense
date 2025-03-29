namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PortworxVolumeSource
    {
        public string? FsType { get; set; }
        public string? VolumeID { get; set; }
        public bool? ReadOnly { get; set; }
    }
}