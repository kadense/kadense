namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1WeightedPodAffinityTerm
    {
        public int? Weight { get; set; }
        public V1PodAffinityTerm? PodAffinityTerm { get; set; }
    }
}