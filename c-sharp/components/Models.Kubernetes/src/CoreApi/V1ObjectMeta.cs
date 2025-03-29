namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ObjectMeta
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public string? Uid { get; set; }
        public Dictionary<string, string>? Labels { get; set; }
        public Dictionary<string, string>? Annotations { get; set; }
        public string? CreationTimestamp { get; set; }
        public string? ResourceVersion { get; set; }
        public string? SelfLink { get; set; }
        public int? Generation { get; set; }
        public int? DeletionGracePeriodSeconds { get; set; }
        public bool? DeletionTimestamp { get; set; }
        public List<string>? Finalizers { get; set; }
        public List<V1OwnerReference>? OwnerReferences { get; set; }
        public string? GenerateName { get; set; }
        public List<V1ManagedFieldsEntry>? ManagedFields { get; set; }
    }
}