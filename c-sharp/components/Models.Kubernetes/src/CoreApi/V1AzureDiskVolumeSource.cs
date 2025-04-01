namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureDiskVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1AzureDiskVolumeSource>
    {
        public string? CachingMode { get; set; }
        public string? DiskName { get; set; }
        public string? DiskURI { get; set; }
        public string? FSType { get; set; }
        public string? Kind { get; set; }
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