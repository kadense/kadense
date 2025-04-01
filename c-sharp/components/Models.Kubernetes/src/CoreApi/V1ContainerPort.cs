
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ContainerPort : KadenseTemplatedCopiedResource<k8s.Models.V1ContainerPort>
    {
        public int? ContainerPort { get; set; }
        public string? Protocol { get; set; }
        public string? HostIP { get; set; }
        public int? HostPort { get; set; }
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