namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EnvVar : KadenseTemplatedCopiedResource<k8s.Models.V1EnvVar>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("valueFrom")]
        public V1EnvVarSource? ValueFrom { get; set; }

        public override k8s.Models.V1EnvVar ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EnvVar(
                name: this.GetValue(this.Name, variables),
                value: this.GetValue(this.Value, variables),
                valueFrom: this.ValueFrom != null ? this.ValueFrom.ToOriginal(variables) : null
            );
        }
    }
}