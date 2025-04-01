using Kadense.Models.Kubernetes;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LifecycleHandler : KadenseTemplatedCopiedResource<k8s.Models.V1LifecycleHandler>
    {
        [JsonPropertyName("_exec")]
        public V1ExecAction? ExecAction { get; set; }
        public V1HTTPGetAction? HttpGet { get; set; }
        public V1TCPSocketAction? TcpSocket { get; set; }
        public V1SleepAction? Sleep { get; set; }

        public override k8s.Models.V1LifecycleHandler ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1LifecycleHandler(
                exec: this.ExecAction != null ? this.ExecAction.ToOriginal(variables) : null,
                httpGet: this.HttpGet != null ? this.HttpGet.ToOriginal(variables) : null,
                tcpSocket: this.TcpSocket != null ? this.TcpSocket.ToOriginal(variables) : null,
                sleep: this.Sleep != null ? this.Sleep.ToOriginal(variables) : null
            );
        }
    }
}