namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HTTPGetAction : KadenseTemplatedCopiedResource<k8s.Models.V1HTTPGetAction>
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("scheme")]
        public string? Scheme { get; set; }

        [JsonPropertyName("httpHeaders")]
        public List<V1HTTPHeader>? HttpHeaders { get; set; }

        public override k8s.Models.V1HTTPGetAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1HTTPGetAction(
                path: this.GetValue(this.Path, variables),
                port: this.Port,
                host: this.GetValue(this.Host, variables),
                scheme: this.GetValue(this.Scheme, variables),
                httpHeaders: this.GetValue<V1HTTPHeader, k8s.Models.V1HTTPHeader>(this.HttpHeaders, variables)
            );
        }
    }
}