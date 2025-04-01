
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeProjection : KadenseTemplatedCopiedResource<k8s.Models.V1VolumeProjection>
    {
        public V1ClusterTrustBundleProjection? ClusterTrustBundle { get; set; }
        public V1ConfigMapProjection? ConfigMap { get; set; }
        public V1DownwardAPIProjection? DownwardAPI { get; set; }
        public V1SecretProjection? Secret { get; set; }
        public V1ServiceAccountTokenProjection? ServiceAccountToken { get; set; }

        public override k8s.Models.V1VolumeProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1VolumeProjection(
                clusterTrustBundle: this.ClusterTrustBundle != null ? this.ClusterTrustBundle.ToOriginal(variables) : null,
                configMap: this.ConfigMap != null ? this.ConfigMap.ToOriginal(variables) : null,
                downwardAPI: this.DownwardAPI != null ? this.DownwardAPI.ToOriginal(variables) : null,
                secret: this.Secret != null ? this.Secret.ToOriginal(variables) : null,
                serviceAccountToken: this.ServiceAccountToken != null ? this.ServiceAccountToken.ToOriginal(variables) : null
            );
        }
    }
}