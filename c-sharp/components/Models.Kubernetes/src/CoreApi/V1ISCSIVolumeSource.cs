namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ISCSIVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ISCSIVolumeSource>
    {
        [JsonPropertyName("chapAuthDiscovery")]
        public bool? ChapAuthDiscovery { get; set; }

        [JsonPropertyName("chapAuthSession")]
        public bool? ChapAuthSession { get; set; }

        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("initiatorName")]
        public string? InitiatorName { get; set; }

        [JsonPropertyName("iqn")]
        public string? Iqn { get; set; }

        [JsonPropertyName("iscsiInterfaceName")]
        public string? IscsiInterfaceName { get; set; }

        [JsonPropertyName("lun")]
        public int? Lun { get; set; }

        [JsonPropertyName("portals")]
        public List<string>? Portals { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("secretRef")]
        public V1LocalObjectReference? SecretRef { get; set; }

        [JsonPropertyName("targetPortal")]
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