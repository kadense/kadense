namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodAffinityTerm
    {
        public V1LabelSelector? LabelSelector { get; set; }
        public List<string>? Namespaces { get; set; }
        public string? TopologyKey { get; set; }
        public List<string>? MatchLabelKeys { get; set; }
        public List<string>? MismatchLabelKeys { get; set; }
        public V1LabelSelector? NamespaceSelector { get; set; }
    }
}