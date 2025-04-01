namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VsphereVirtualDiskVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1VsphereVirtualDiskVolumeSource>
    {
        [JsonPropertyName("volumePath")]
        public string? VolumePath { get; set; }

        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("storagePolicyName")]
        public string? StoragePolicyName { get; set; }

        [JsonPropertyName("storagePolicyID")]
        public string? StoragePolicyID { get; set; }

        public override k8s.Models.V1VsphereVirtualDiskVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1VsphereVirtualDiskVolumeSource(
                volumePath: this.GetValue(this.VolumePath, variables),
                fsType: this.GetValue(this.FsType, variables),
                storagePolicyID: this.GetValue(this.StoragePolicyID, variables),
                storagePolicyName: this.GetValue(this.StoragePolicyName, variables)
            );
        }
    }
}