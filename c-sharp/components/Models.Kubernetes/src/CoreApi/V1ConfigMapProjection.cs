using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapProjection : KadenseTemplatedCopiedResource<k8s.Models.V1ConfigMapProjection>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }

        [JsonPropertyName("items")]
        public List<V1KeyToPath>? Items { get; set; }

        public override k8s.Models.V1ConfigMapProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ConfigMapProjection(
                name: this.GetValue(this.Name, variables),
                optional: this.Optional,
                items: this.Items != null ? this.Items.Select(i => i.ToOriginal(variables)).ToList() : null
            );
        }
    }
}