namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GRPCAction : KadenseTemplatedCopiedResource<k8s.Models.V1GRPCAction>
    {
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("service")]
        public string? Service { get; set; }

        public override k8s.Models.V1GRPCAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1GRPCAction(
                port: Port!.Value,
                service: this.GetValue(this.Service, variables)
            );
        }
    }
}