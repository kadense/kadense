namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeAffinity : KadenseTemplatedCopiedResource<k8s.Models.V1NodeAffinity>
    {
        public List<V1PreferredSchedulingTerm>? PreferredDuringSchedulingIgnoredDuringExecution { get; set; }
        public V1NodeSelector? RequiredDuringSchedulingIgnoredDuringExecution { get; set; }

        public override k8s.Models.V1NodeAffinity ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1NodeAffinity(
                preferredDuringSchedulingIgnoredDuringExecution: this.GetValue<V1PreferredSchedulingTerm, k8s.Models.V1PreferredSchedulingTerm>(this.PreferredDuringSchedulingIgnoredDuringExecution, variables),
                requiredDuringSchedulingIgnoredDuringExecution: this.RequiredDuringSchedulingIgnoredDuringExecution != null ? this.RequiredDuringSchedulingIgnoredDuringExecution.ToOriginal(variables) : null
            );
        }
    }
}