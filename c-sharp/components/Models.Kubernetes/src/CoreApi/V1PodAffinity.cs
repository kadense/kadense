namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodAffinity : KadenseTemplatedCopiedResource<k8s.Models.V1PodAffinity>
    {
        public List<V1PodAffinityTerm>? RequiredDuringSchedulingIgnoredDuringExecution { get; set; }
        public List<V1WeightedPodAffinityTerm>? PreferredDuringSchedulingIgnoredDuringExecution { get; set; }

        public override k8s.Models.V1PodAffinity ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodAffinity(
                preferredDuringSchedulingIgnoredDuringExecution: this.GetValue<V1WeightedPodAffinityTerm, k8s.Models.V1WeightedPodAffinityTerm>(this.PreferredDuringSchedulingIgnoredDuringExecution, variables),
                requiredDuringSchedulingIgnoredDuringExecution: this.GetValue<V1PodAffinityTerm, k8s.Models.V1PodAffinityTerm>(this.RequiredDuringSchedulingIgnoredDuringExecution, variables)
            );
        }
    }
}