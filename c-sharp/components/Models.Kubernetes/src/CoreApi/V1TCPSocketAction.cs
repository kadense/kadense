namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TCPSocketAction : KadenseTemplatedCopiedResource<k8s.Models.V1TCPSocketAction>
    {
        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("port")]
        public string? Port { get; set; }

        public override k8s.Models.V1TCPSocketAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1TCPSocketAction(
                port: this.Port != null ? new k8s.Models.IntstrIntOrString(this.GetValue(this.Port, variables)) : null,
                host: this.GetValue(this.Host, variables)
            );
        }
    }
}