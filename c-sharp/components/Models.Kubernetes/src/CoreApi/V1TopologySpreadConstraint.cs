namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TopologySpreadConstraint
    {
        public string? TopologyKey { get; set; }
        public int? MaxSkew { get; set; }
        public string? WhenUnsatisfiable { get; set; }
        public V1LabelSelector? LabelSelector { get; set; }
        public List<string>? MatchLabelKeys { get; set; }
        public int? MinDomains { get; set; }
        public string? NodeAffinityPolicy { get; set; }
        public string? NodeTaintsPolicy { get; set; }
    }
}