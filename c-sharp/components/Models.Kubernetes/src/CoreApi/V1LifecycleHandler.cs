using Kadense.Models.Kubernetes;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LifecycleHandler
    {
        [JsonPropertyName("_exec")]
        public V1ExecAction? ExecAction { get; set; }
        public V1HTTPGetAction? HttpGet { get; set; }
        public V1TCPSocketAction? TcpSocket { get; set; }
        public V1SleepAction? Sleep { get; set; }
    }
}