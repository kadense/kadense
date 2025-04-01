namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Affinity : KadenseTemplatedCopiedResource<k8s.Models.V1Affinity>
    { 
        public V1NodeAffinity? NodeAffinity { get; set; }
        public V1PodAffinity? PodAffinity { get; set; }
        public V1PodAntiAffinity? PodAntiAffinity { get; set; }

        public override k8s.Models.V1Affinity ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Affinity(
                this.NodeAffinity != null ? this.NodeAffinity.ToOriginal(variables) : null,
                this.PodAffinity != null ? this.PodAffinity.ToOriginal(variables) : null,
                this.PodAntiAffinity != null ? this.PodAntiAffinity.ToOriginal(variables) : null
            );
            
        }
    }
}