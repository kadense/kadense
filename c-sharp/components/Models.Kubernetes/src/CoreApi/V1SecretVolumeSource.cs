namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretVolumeSource
    {
        public string? SecretName { get; set; }
        public bool? Optional { get; set; }
        public int? DefaultMode { get; set; }
        public List<V1KeyToPath>? Items { get; set; }
    }
}