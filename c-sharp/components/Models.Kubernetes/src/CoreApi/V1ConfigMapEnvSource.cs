using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapEnvSource : KadenseTemplatedCopiedResource<k8s.Models.V1ConfigMapEnvSource>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }

        public override k8s.Models.V1ConfigMapEnvSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ConfigMapEnvSource(
                name: this.GetValue(this.Name, variables),
                optional: this.Optional
            );
        }
    }
}