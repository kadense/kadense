namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureDiskVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1AzureDiskVolumeSource>
    {
        [JsonPropertyName("cachingMode")]
        public string? CachingMode { get; set; }

        [JsonPropertyName("diskName")]
        public string? DiskName { get; set; }

        [JsonPropertyName("diskURI")]
        public string? DiskURI { get; set; }

        [JsonPropertyName("fsType")]
        public string? FSType { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1AzureDiskVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1AzureDiskVolumeSource(
                cachingMode: this.GetValue(CachingMode,variables),
                diskName: this.GetValue(this.DiskName, variables),
                diskURI : this.GetValue(this.DiskURI, variables),
                fsType: this.GetValue(this.FSType, variables),
                kind: this.GetValue(this.Kind, variables),
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}