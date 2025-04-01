namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodAntiAffinity : KadenseTemplatedCopiedResource<k8s.Models.V1PodAntiAffinity>
    {
        [JsonPropertyName("requiredDuringSchedulingIgnoredDuringExecution")]
        public List<V1PodAffinityTerm>? RequiredDuringSchedulingIgnoredDuringExecution { get; set; }

        [JsonPropertyName("preferredDuringSchedulingIgnoredDuringExecution")]
        public List<V1WeightedPodAffinityTerm>? PreferredDuringSchedulingIgnoredDuringExecution { get; set; }

        public override k8s.Models.V1PodAntiAffinity ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodAntiAffinity(
                preferredDuringSchedulingIgnoredDuringExecution: this.GetValue<V1WeightedPodAffinityTerm, k8s.Models.V1WeightedPodAffinityTerm>(this.PreferredDuringSchedulingIgnoredDuringExecution, variables),
                requiredDuringSchedulingIgnoredDuringExecution: this.GetValue<V1PodAffinityTerm, k8s.Models.V1PodAffinityTerm>(this.RequiredDuringSchedulingIgnoredDuringExecution, variables)
            );
        }
    }
}