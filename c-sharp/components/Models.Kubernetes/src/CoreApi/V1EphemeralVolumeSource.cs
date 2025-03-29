namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EphemeralVolumeSource
    {
        public V1PersistentVolumeClaimTemplate? VolumeClaimTemplate { get; set; }
    }
}