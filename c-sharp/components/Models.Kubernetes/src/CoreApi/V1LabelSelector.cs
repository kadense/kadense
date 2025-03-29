namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LabelSelector
    {
        public Dictionary<string, string>? MatchLabels { get; set; }
        public V1LabelSelectorRequirement? MatchExpressions { get; set; }
    }
}