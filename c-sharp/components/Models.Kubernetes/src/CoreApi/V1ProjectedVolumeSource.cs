namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ProjectedVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ProjectedVolumeSource>
    {
        [JsonPropertyName("defaultMode")]
        public int? DefaultMode { get; set; }

        [JsonPropertyName("sources")]
        public List<V1VolumeProjection>? Sources { get; set; }

        public override k8s.Models.V1ProjectedVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ProjectedVolumeSource(
                defaultMode: this.DefaultMode,
                sources: this.GetValue<V1VolumeProjection, k8s.Models.V1VolumeProjection>(this.Sources, variables)
            );
        }
    }
}