namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodDNSConfig : KadenseTemplatedCopiedResource<k8s.Models.V1PodDNSConfig>
    {
        [JsonPropertyName("nameservers")]
        public List<string>? Nameservers { get; set; }

        [JsonPropertyName("searches")]
        public List<string>? Searches { get; set; }

        [JsonPropertyName("options")]
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