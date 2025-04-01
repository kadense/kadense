namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodAffinityTerm : KadenseTemplatedCopiedResource<k8s.Models.V1PodAffinityTerm>
    {
        [JsonPropertyName("labelSelector")]
        public V1LabelSelector? LabelSelector { get; set; }

        [JsonPropertyName("namespaces")]
        public List<string>? Namespaces { get; set; }

        [JsonPropertyName("topologyKey")]
        public string? TopologyKey { get; set; }

        [JsonPropertyName("matchLabelKeys")]
        public List<string>? MatchLabelKeys { get; set; }

        [JsonPropertyName("mismatchLabelKeys")]
        public List<string>? MismatchLabelKeys { get; set; }

        [JsonPropertyName("namespaceSelector")]
        public V1LabelSelector? NamespaceSelector { get; set; }

        public override k8s.Models.V1PodAffinityTerm ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodAffinityTerm(
                labelSelector: this.LabelSelector != null ? this.LabelSelector.ToOriginal(variables) : null,
                namespaces: this.GetValue(this.Namespaces, variables),
                topologyKey: this.GetValue(this.TopologyKey, variables),
                matchLabelKeys: this.GetValue(this.MatchLabelKeys, variables),
                mismatchLabelKeys: this.GetValue(this.MismatchLabelKeys, variables),
                namespaceSelector: this.NamespaceSelector != null ? this.NamespaceSelector.ToOriginal(variables) : null
            );
        }
    }
}