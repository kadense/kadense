namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EnvFromSource : KadenseTemplatedCopiedResource<k8s.Models.V1EnvFromSource>
    {
        [JsonPropertyName("configMapRef")]
        public V1ConfigMapEnvSource? ConfigMapRef { get; set; }

        [JsonPropertyName("secretRef")]
        public V1SecretEnvSource? SecretRef { get; set; }

        [JsonPropertyName("prefix")]
        public string? Prefix { get; set; }

        public override k8s.Models.V1EnvFromSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EnvFromSource(
                configMapRef: this.ConfigMapRef != null ? this.ConfigMapRef.ToOriginal(variables) : null,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                prefix: this.GetValue(this.Prefix, variables)
            );
        }
    }
}