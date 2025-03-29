namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Affinity
    { 
        public V1NodeAffinity? NodeAffinity { get; set; }
        public V1PodAffinity? PodAffinity { get; set; }
        public V1PodAntiAffinity? PodAntiAffinity { get; set; }
    }
}