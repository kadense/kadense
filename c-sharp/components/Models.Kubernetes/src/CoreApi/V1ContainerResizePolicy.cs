namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ContainerResizePolicy
    {
        public string? ResourceName { get; set; }
        public string? RestartPolicy { get; set; }
    }
}