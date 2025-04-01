
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ContainerResizePolicy : KadenseTemplatedCopiedResource<k8s.Models.V1ContainerResizePolicy>
    {
        public string? ResourceName { get; set; }
        public string? RestartPolicy { get; set; }

        public override k8s.Models.V1ContainerResizePolicy ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ContainerResizePolicy(
                resourceName: this.GetValue(this.ResourceName, variables),
                restartPolicy: this.GetValue(this.RestartPolicy, variables)
            );
        }
    }
}