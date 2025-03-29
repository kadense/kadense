namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodDNSConfig
    {
        public string? Nameserver { get; set; }
        public List<string>? Searches { get; set; }
        public List<V1PodDNSConfigOption>? Options { get; set; }
    }
}