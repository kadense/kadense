namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TopologySpreadConstraint : KadenseTemplatedCopiedResource<k8s.Models.V1TopologySpreadConstraint>
    {
        public string? TopologyKey { get; set; }
        public int? MaxSkew { get; set; }
        public string? WhenUnsatisfiable { get; set; }
        public V1LabelSelector? LabelSelector { get; set; }
        public List<string>? MatchLabelKeys { get; set; }
        public int? MinDomains { get; set; }
        public string? NodeAffinityPolicy { get; set; }
        public string? NodeTaintsPolicy { get; set; }

        public override k8s.Models.V1TopologySpreadConstraint ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1TopologySpreadConstraint(
                topologyKey: this.GetValue(this.TopologyKey, variables),
                maxSkew: this.MaxSkew!.Value,
                whenUnsatisfiable: this.GetValue(this.WhenUnsatisfiable, variables),
                labelSelector: this.LabelSelector != null ? this.LabelSelector.ToOriginal(variables) : null,
                matchLabelKeys: this.GetValue(this.MatchLabelKeys, variables),
                minDomains: this.MinDomains,
                nodeAffinityPolicy: this.GetValue(this.NodeAffinityPolicy, variables),
                nodeTaintsPolicy: this.GetValue(this.NodeTaintsPolicy, variables)
            );
        }
    }
}