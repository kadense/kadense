namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PersistentVolumeClaimTemplate : KadenseTemplatedCopiedResource<k8s.Models.V1PersistentVolumeClaimTemplate>
    {
        [JsonPropertyName("metadata")]
        public V1ObjectMeta? Metadata { get; set; }

        [JsonPropertyName("spec")]
        public V1PersistentVolumeClaimSpec? Spec { get; set; }

        public override k8s.Models.V1PersistentVolumeClaimTemplate ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PersistentVolumeClaimTemplate(
                spec: this.Spec != null ? this.Spec.ToOriginal(variables) : null,
                metadata: this.Metadata != null ? this.Metadata.ToOriginal(variables) : null
            );
        }
    }
}