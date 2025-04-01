namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeSelectorTerm : KadenseTemplatedCopiedResource<k8s.Models.V1NodeSelectorTerm>
    {
        [JsonPropertyName("matchExpressions")]
        public List<V1NodeSelectorRequirement>? MatchExpressions { get; set; }

        [JsonPropertyName("matchFields")]
        public List<V1NodeSelectorRequirement>? MatchFields { get; set; }

        public override k8s.Models.V1NodeSelectorTerm ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1NodeSelectorTerm(
                matchExpressions: this.GetValue<V1NodeSelectorRequirement, k8s.Models.V1NodeSelectorRequirement>(this.MatchExpressions, variables),
                matchFields: this.GetValue<V1NodeSelectorRequirement, k8s.Models.V1NodeSelectorRequirement>(this.MatchFields, variables)
            );
        }
    }
}