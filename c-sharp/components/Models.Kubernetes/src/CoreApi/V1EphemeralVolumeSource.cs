namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EphemeralVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1EphemeralVolumeSource>
    {
        [JsonPropertyName("volumeClaimTemplate")]
        public V1PersistentVolumeClaimTemplate? VolumeClaimTemplate { get; set; }

        public override k8s.Models.V1EphemeralVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EphemeralVolumeSource(
                volumeClaimTemplate: this.VolumeClaimTemplate != null ? this.VolumeClaimTemplate.ToOriginal(variables) : null
            );
        }
    }
}