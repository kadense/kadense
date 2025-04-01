using Kadense.Models.Kubernetes;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Probe : KadenseTemplatedCopiedResource<k8s.Models.V1Probe>
    {
        [JsonPropertyName("_exec")]
        public V1ExecAction? Exec { get; set; }
        public int? FailureThreshold { get; set; }
        public V1GRPCAction? Grpc { get; set; }
        public V1HTTPGetAction? HttpGet { get; set; }
        public int? InitialDelaySeconds { get; set; }
        public int? PeriodSeconds { get; set; }
        public int? SuccessThreshold { get; set; }
        public V1TCPSocketAction? TcpSocket { get; set; }
        public int? TimeoutSeconds { get; set; }
        public int? TerminationGracePeriodSeconds { get; set; }

        public override k8s.Models.V1Probe ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Probe(
                exec: this.Exec != null ? this.Exec.ToOriginal(variables) : null,
                failureThreshold: this.FailureThreshold,
                grpc: this.Grpc != null ? this.Grpc.ToOriginal(variables) : null,
                httpGet: this.HttpGet != null ? this.HttpGet.ToOriginal(variables) : null,
                initialDelaySeconds: this.InitialDelaySeconds,
                periodSeconds: this.PeriodSeconds,
                successThreshold: this.SuccessThreshold,
                tcpSocket: this.TcpSocket != null ? this.TcpSocket.ToOriginal(variables) : null,
                timeoutSeconds: this.TimeoutSeconds,
                terminationGracePeriodSeconds: this.TerminationGracePeriodSeconds
            );
        }
    }
}