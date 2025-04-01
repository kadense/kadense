namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TypedObjectReference : KadenseTemplatedCopiedResource<k8s.Models.V1TypedObjectReference>
    {
        [JsonPropertyName("apiGroup")]
        public string? ApiGroup { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        public override k8s.Models.V1TypedObjectReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1TypedObjectReference(
                namespaceProperty: this.GetValue(this.Namespace, variables),
                kind: this.GetValue(this.Kind, variables),
                name: this.GetValue(this.Name, variables),
                apiGroup: this.GetValue(this.ApiGroup, variables)
            );
        }
    }
}