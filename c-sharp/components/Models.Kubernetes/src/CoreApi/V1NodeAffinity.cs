namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeAffinity
    {
        public List<V1PreferredSchedulingTerm>? PreferredDuringSchedulingIgnoredDuringExecution { get; set; }
        public V1NodeSelector? RequiredDuringSchedulingIgnoredDuringExecution { get; set; }
    }
}