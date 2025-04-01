
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ClusterTrustBundleProjection : KadenseTemplatedCopiedResource<k8s.Models.V1ClusterTrustBundleProjection>
    { 
        public V1LabelSelector? LabelSelector { get; set; }
        public string? Name { get; set; }
        public bool? Optional { get; set; }
        public string? Path { get; set; }
        public string? SignerName { get; set; }

        public override k8s.Models.V1ClusterTrustBundleProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ClusterTrustBundleProjection(
                labelSelector: this.LabelSelector != null ? this.LabelSelector.ToOriginal(variables) : null,
                name: this.GetValue(this.Name, variables),
                optional: this.Optional,
                path: this.GetValue(this.Path, variables),
                signerName: this.GetValue(this.SignerName, variables)
            );
        }
    }
}