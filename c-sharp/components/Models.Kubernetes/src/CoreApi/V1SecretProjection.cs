namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretProjection : KadenseTemplatedCopiedResource<k8s.Models.V1SecretProjection>
    {
        [JsonPropertyName("items")]
        public List<V1KeyToPath>? Items { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }

        public override k8s.Models.V1SecretProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SecretProjection(
                items: this.GetValue<V1KeyToPath, k8s.Models.V1KeyToPath>(this.Items, variables),
                name: this.GetValue(this.Name, variables),
                optional: this.Optional
            );
        }
    }
}