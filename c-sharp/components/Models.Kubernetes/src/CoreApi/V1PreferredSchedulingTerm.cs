namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PreferredSchedulingTerm : KadenseTemplatedCopiedResource<k8s.Models.V1PreferredSchedulingTerm>
    {
        [JsonPropertyName("weight")]
        public int? Weight { get; set; }

        [JsonPropertyName("preference")]
        public V1NodeSelectorTerm? Preference { get; set; }

        public override k8s.Models.V1PreferredSchedulingTerm ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PreferredSchedulingTerm(
                weight: this.Weight!.Value,
                preference: this.Preference!.ToOriginal(variables)
            );
        }
    }
}