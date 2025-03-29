namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapVolumeSource
    {
        public string? Name { get; set; }
        public bool? Optional { get; set; }
        public int? DefaultMode { get; set; }
        public List<V1KeyToPath>? Items { get; set; }
    }
}