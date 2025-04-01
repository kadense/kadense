namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HostAlias : KadenseTemplatedCopiedResource<k8s.Models.V1HostAlias>
    {
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        [JsonPropertyName("hostnames")]
        public List<string>? Hostnames { get; set; }

        public override k8s.Models.V1HostAlias ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1HostAlias(
                ip: this.GetValue(this.Ip, variables),
                hostnames: this.GetValue(this.Hostnames, variables)
            );
        }
    }
}