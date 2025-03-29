namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ClusterTrustBundleProjection
    { 
        public V1LabelSelector? LabelSelector { get; set; }
        public string? Name { get; set; }
        public bool? Optional { get; set; }
        public string? Path { get; set; }
        public string? SignerName { get; set; }
    }
}