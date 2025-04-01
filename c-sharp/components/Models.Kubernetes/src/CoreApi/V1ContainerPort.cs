using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ContainerPort : KadenseTemplatedCopiedResource<k8s.Models.V1ContainerPort>
    {
        [JsonPropertyName("containerPort")]
        public int? ContainerPort { get; set; }

        [JsonPropertyName("protocol")]
        public string? Protocol { get; set; }

        [JsonPropertyName("hostIP")]
        public string? HostIP { get; set; }

        [JsonPropertyName("hostPort")]
        public int? HostPort { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public override k8s.Models.V1ContainerPort ToOriginal(Dictionary<string, string> variables)
        {
            if(this.ContainerPort == null)
                throw new NullReferenceException("ContainerPort cannot be null");

            return new k8s.Models.V1ContainerPort(
                containerPort: this.ContainerPort.Value,
                protocol: this.GetValue(this.Protocol, variables),
                hostIP: this.GetValue(this.HostIP, variables),
                hostPort: this.HostPort,
                name: this.GetValue(this.Name, variables)
            );
        }
    }
}