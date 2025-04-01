
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GRPCAction : KadenseTemplatedCopiedResource<k8s.Models.V1GRPCAction>
    {
        public int? Port { get; set; }
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