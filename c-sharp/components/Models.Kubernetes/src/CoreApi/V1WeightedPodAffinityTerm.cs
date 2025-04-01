
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1WeightedPodAffinityTerm : KadenseTemplatedCopiedResource<k8s.Models.V1WeightedPodAffinityTerm>
    {
        public int? Weight { get; set; }
        public V1PodAffinityTerm? PodAffinityTerm { get; set; }

        public override k8s.Models.V1WeightedPodAffinityTerm ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1WeightedPodAffinityTerm(
                weight: this.Weight!.Value,
                podAffinityTerm: this.PodAffinityTerm != null ? this.PodAffinityTerm.ToOriginal(variables) : null
            );
        }
    }
}