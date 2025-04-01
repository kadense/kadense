namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LabelSelector : KadenseTemplatedCopiedResource<k8s.Models.V1LabelSelector>
    {
        public Dictionary<string, string>? MatchLabels { get; set; }
        public List<V1LabelSelectorRequirement>? MatchExpressions { get; set; }

        public override k8s.Models.V1LabelSelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1LabelSelector(
                matchLabels: this.GetValue(this.MatchLabels, variables),
                matchExpressions: this.MatchExpressions != null ? this.MatchExpressions.Select(i => i.ToOriginal(variables)).ToList() : null
            );
        }
    }
}