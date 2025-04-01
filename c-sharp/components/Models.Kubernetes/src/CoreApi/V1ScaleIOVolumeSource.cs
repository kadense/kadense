namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ScaleIOVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ScaleIOVolumeSource>
    {
        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("gateway")]
        public string? Gateway { get; set; }

        [JsonPropertyName("protectionDomain")]
        public string? ProtectionDomain { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("secretRef")]
        public V1LocalObjectReference? SecretRef { get; set; }

        [JsonPropertyName("sslEnabled")]
        public bool? SslEnabled { get; set; }

        [JsonPropertyName("storageMode")]
        public string? StorageMode { get; set; }

        [JsonPropertyName("storagePool")]
        public string? StoragePool { get; set; }

        [JsonPropertyName("system")]
        public string? System { get; set; }

        [JsonPropertyName("volumeName")]
        public string? VolumeName { get; set; }

        public override k8s.Models.V1ScaleIOVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ScaleIOVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                gateway: this.GetValue(this.Gateway, variables),
                protectionDomain: this.GetValue(this.ProtectionDomain, variables),
                readOnlyProperty: this.ReadOnly,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                sslEnabled: this.SslEnabled,
                storageMode: this.GetValue(this.StorageMode, variables),
                storagePool: this.GetValue(this.StoragePool, variables),
                system: this.GetValue(this.System, variables),
                volumeName: this.GetValue(this.VolumeName, variables)
            );
        }
    }
}