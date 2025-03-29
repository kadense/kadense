using Kadense.Models.Kubernetes;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Probe
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
    }
}