namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodDNSConfig : KadenseTemplatedCopiedResource<k8s.Models.V1PodDNSConfig>
    {
        public List<string>? Nameservers { get; set; }
        public List<string>? Searches { get; set; }
        public List<V1PodDNSConfigOption>? Options { get; set; }

        public override k8s.Models.V1PodDNSConfig ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodDNSConfig(
                nameservers: this.GetValue(this.Nameservers, variables),
                searches: this.GetValue(this.Searches, variables),
                options: this.GetValue<V1PodDNSConfigOption, k8s.Models.V1PodDNSConfigOption>(this.Options, variables)
            );
        }
    }
}