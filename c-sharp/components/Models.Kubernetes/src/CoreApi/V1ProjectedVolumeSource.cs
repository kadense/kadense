namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ProjectedVolumeSource
    {
        public int? DefaultMode { get; set; }
        public List<V1VolumeProjection>? Sources { get; set; }
    }
}