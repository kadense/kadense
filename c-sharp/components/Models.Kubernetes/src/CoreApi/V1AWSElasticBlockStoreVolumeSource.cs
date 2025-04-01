namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AWSElasticBlockStoreVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1AWSElasticBlockStoreVolumeSource>
    { 
        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("volumeID")]
        public string? VolumeID { get; set; }

        [JsonPropertyName("partition")]
        public int? Partition { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1AWSElasticBlockStoreVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1AWSElasticBlockStoreVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                volumeID: this.GetValue(this.VolumeID, variables),
                partition: this.Partition,
                readOnlyProperty: this.ReadOnly   
            );
        }
    }
}