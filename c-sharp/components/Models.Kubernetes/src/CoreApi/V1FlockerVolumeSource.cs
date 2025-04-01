namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1FlockerVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1FlockerVolumeSource>
    {
        [JsonPropertyName("datasetName")]
        public string? DatasetName { get; set; }

        [JsonPropertyName("datasetUUID")]
        public string? DatasetUUID { get; set; }

        public override k8s.Models.V1FlockerVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1FlockerVolumeSource(
                datasetName: this.GetValue(this.DatasetName, variables),
                datasetUUID: this.GetValue(this.DatasetUUID, variables)
            );
        }
    }
}