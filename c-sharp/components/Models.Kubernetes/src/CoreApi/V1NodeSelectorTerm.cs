namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeSelectorTerm
    {
        public List<V1NodeSelectorRequirement>? MatchExpressions { get; set; }
        public List<V1NodeSelectorRequirement>? MatchFields { get; set; }
    }
}