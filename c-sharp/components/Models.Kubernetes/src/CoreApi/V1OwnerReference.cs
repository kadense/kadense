namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1OwnerReference : KadenseTemplatedCopiedResource<k8s.Models.V1OwnerReference>
    {
        [JsonPropertyName("apiVersion")]
        public string? ApiVersion { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("uid")]
        public string? Uid { get; set; }

        [JsonPropertyName("controller")]
        public bool? Controller { get; set; }

        [JsonPropertyName("blockOwnerDeletion")]
        public bool? BlockOwnerDeletion { get; set; }

        public override k8s.Models.V1OwnerReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1OwnerReference(
                apiVersion: this.GetValue(this.ApiVersion, variables),
                kind: this.GetValue(this.Kind, variables),
                name: this.GetValue(this.Name, variables),
                uid: this.GetValue(this.Uid, variables),
                blockOwnerDeletion: this.BlockOwnerDeletion,
                controller: this.Controller
            );
        }
    }
}