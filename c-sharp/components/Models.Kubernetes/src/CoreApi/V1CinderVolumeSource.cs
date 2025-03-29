namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CinderVolumeSource
    { 
        public string? FsType { get; set; }
        public string? VolumeID { get; set; }
        public bool? ReadOnly { get; set; }
        public string? SecretRef { get; set; }
    }
}