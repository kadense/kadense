using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapKeySelector : KadenseTemplatedCopiedResource<k8s.Models.V1ConfigMapKeySelector>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }

        public override k8s.Models.V1ConfigMapKeySelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ConfigMapKeySelector(
                name: this.GetValue(this.Name, variables),
                key: this.GetValue(this.Key, variables),
                optional: this.Optional
            );
        }
    }
}