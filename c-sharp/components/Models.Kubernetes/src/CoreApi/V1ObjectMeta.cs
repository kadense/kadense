namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ObjectMeta : KadenseTemplatedCopiedResource<k8s.Models.V1ObjectMeta>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("uid")]
        public string? Uid { get; set; }

        [JsonPropertyName("labels")]
        public Dictionary<string, string>? Labels { get; set; }

        [JsonPropertyName("annotations")]
        public Dictionary<string, string>? Annotations { get; set; }

        [JsonPropertyName("creationTimestamp")]
        public string? CreationTimestamp { get; set; }

        [JsonPropertyName("resourceVersion")]
        public string? ResourceVersion { get; set; }

        [JsonPropertyName("selfLink")]
        public string? SelfLink { get; set; }

        [JsonPropertyName("generation")]
        public long? Generation { get; set; }

        [JsonPropertyName("deletionGracePeriodSeconds")]
        public long? DeletionGracePeriodSeconds { get; set; }

        [JsonPropertyName("deletionTimestamp")]
        public string? DeletionTimestamp { get; set; }

        [JsonPropertyName("finalizers")]
        public List<string>? Finalizers { get; set; }

        [JsonPropertyName("ownerReferences")]
        public List<V1OwnerReference>? OwnerReferences { get; set; }

        [JsonPropertyName("generateName")]
        public string? GenerateName { get; set; }

        [JsonPropertyName("managedFields")]
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