namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeSelector : KadenseTemplatedCopiedResource<k8s.Models.V1NodeSelector>
    {
        public List<V1NodeSelectorTerm>? NodeSelectorTerms { get; set; }

        public override k8s.Models.V1NodeSelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1NodeSelector(
                nodeSelectorTerms: this.GetValue<V1NodeSelectorTerm, k8s.Models.V1NodeSelectorTerm>(this.NodeSelectorTerms, variables)
            );
        }
    }
}