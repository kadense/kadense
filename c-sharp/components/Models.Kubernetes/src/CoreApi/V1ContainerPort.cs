namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ContainerPort
    {
        public int? ContainerPort { get; set; }
        public string? Protocol { get; set; }
        public string? HostIP { get; set; }
        public int? HostPort { get; set; }
        public string? Name { get; set; }
    }
}