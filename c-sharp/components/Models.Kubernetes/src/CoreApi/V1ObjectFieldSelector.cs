namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ObjectFieldSelector : KadenseTemplatedCopiedResource<k8s.Models.V1ObjectFieldSelector>
    {
        [JsonPropertyName("apiVersion")]
        public string? ApiVersion { get; set; }

        [JsonPropertyName("fieldPath")]
        public string? FieldPath { get; set; }

        public override k8s.Models.V1ObjectFieldSelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ObjectFieldSelector(
                apiVersion: this.GetValue(this.ApiVersion, variables),
                fieldPath: this.GetValue(this.FieldPath, variables)
            );
        }
    }
}