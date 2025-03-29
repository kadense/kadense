namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ISCSIVolumeSource
    {
        public bool? ChapAuthDiscovery { get; set; }
        public bool? ChapAuthSession { get; set; }
        public string? FsType { get; set; }
        public string? InitiatorName { get; set; }
        public string? Iqn { get; set; }
        public string? IscsiInterfaceName { get; set; }
        public int? Lun { get; set; }
        public List<string>? Portals { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public string? TargetPortal { get; set; }
    }
}