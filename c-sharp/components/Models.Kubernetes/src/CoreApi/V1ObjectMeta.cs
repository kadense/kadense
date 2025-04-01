namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ObjectMeta : KadenseTemplatedCopiedResource<k8s.Models.V1ObjectMeta>
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public string? Uid { get; set; }
        public Dictionary<string, string>? Labels { get; set; }
        public Dictionary<string, string>? Annotations { get; set; }
        public string? CreationTimestamp { get; set; }
        public string? ResourceVersion { get; set; }
        public string? SelfLink { get; set; }
        public long? Generation { get; set; }
        public long? DeletionGracePeriodSeconds { get; set; }
        public string? DeletionTimestamp { get; set; }
        public List<string>? Finalizers { get; set; }
        public List<V1OwnerReference>? OwnerReferences { get; set; }
        public string? GenerateName { get; set; }
        public List<V1ManagedFieldsEntry>? ManagedFields { get; set; }

        public override k8s.Models.V1ObjectMeta ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ObjectMeta(
                annotations: this.GetValue(this.Annotations, variables),
                creationTimestamp: this.CreationTimestamp != null ? DateTime.Parse(this.CreationTimestamp) : null,
                deletionGracePeriodSeconds: this.DeletionGracePeriodSeconds,
                deletionTimestamp: this.DeletionTimestamp != null ? DateTime.Parse(this.DeletionTimestamp) : null,
                finalizers: this.GetValue(this.Finalizers, variables),
                generateName: this.GetValue(this.GenerateName, variables),
                generation: this.Generation,
                labels: this.GetValue(this.Labels, variables),
                managedFields: this.GetValue<V1ManagedFieldsEntry, k8s.Models.V1ManagedFieldsEntry>(this.ManagedFields, variables),
                name: this.GetValue(this.Name, variables),
                namespaceProperty: this.GetValue(this.Namespace, variables),
                ownerReferences: this.GetValue<V1OwnerReference, k8s.Models.V1OwnerReference>(this.OwnerReferences, variables),
                resourceVersion: this.GetValue(this.ResourceVersion, variables),
                selfLink: this.GetValue(this.SelfLink, variables),
                uid: this.GetValue(this.Uid, variables)                
            );
        }
    }
}