namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1OwnerReference
    {
        public string? ApiVersion { get; set; }
        public string? Kind { get; set; }
        public string? Name { get; set; }
        public string? Uid { get; set; }
        public bool? Controller { get; set; }
        public bool? BlockOwnerDeletion { get; set; }
    }
}