namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1StorageOSVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1StorageOSVolumeSource>
    {
        [JsonPropertyName("volumeName")]
        public string? VolumeName { get; set; }

        [JsonPropertyName("volumeNamespace")]
        public string? VolumeNamespace { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("secretRef")]
        public V1LocalObjectReference? SecretRef { get; set; }

        public override k8s.Models.V1StorageOSVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1StorageOSVolumeSource(
                volumeName: this.GetValue(this.VolumeName, variables),
                volumeNamespace: this.GetValue(this.VolumeNamespace, variables),
                readOnlyProperty: this.ReadOnly,
                fsType: this.GetValue(this.FsType, variables),
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null
            );
        }
    }
}