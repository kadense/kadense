namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeProjection
    {
        public V1ClusterTrustBundleProjection? ClusterTrustBundle { get; set; }
        public V1ConfigMapProjection? ConfigMap { get; set; }
        public V1DownwardAPIProjection? DownwardAPI { get; set; }
        public V1SecretProjection? Secret { get; set; }
        public V1ServiceAccountTokenProjection? ServiceAccountToken { get; set; }
    }
}