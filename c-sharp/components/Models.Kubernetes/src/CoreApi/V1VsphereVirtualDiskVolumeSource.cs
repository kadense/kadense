namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VsphereVirtualDiskVolumeSource
    {
        public string? VolumePath { get; set; }
        public string? FsType { get; set; }
        public string? StoragePolicyName { get; set; }
        public string? StoragePolicyID { get; set; }
    }
}