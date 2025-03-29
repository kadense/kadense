namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PersistentVolumeClaimTemplate
    {
        public V1ObjectMeta? Metadata { get; set; }
        public V1PersistentVolumeClaimSpec? Spec { get; set; }
    }
}