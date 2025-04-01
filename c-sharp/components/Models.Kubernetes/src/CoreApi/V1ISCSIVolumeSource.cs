namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ISCSIVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ISCSIVolumeSource>
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

        public override k8s.Models.V1ISCSIVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ISCSIVolumeSource(
                chapAuthDiscovery: this.ChapAuthDiscovery,
                chapAuthSession: this.ChapAuthSession,
                fsType: this.GetValue(this.FsType, variables),
                initiatorName: this.GetValue(this.InitiatorName, variables),
                iqn: this.GetValue(this.Iqn, variables),
                iscsiInterface: this.GetValue(this.IscsiInterfaceName, variables),
                lun: this.Lun!.Value,
                portals: this.GetValue(this.Portals, variables),
                readOnlyProperty: this.ReadOnly,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                targetPortal: this.GetValue(this.TargetPortal, variables)
            );
        }
    }
}