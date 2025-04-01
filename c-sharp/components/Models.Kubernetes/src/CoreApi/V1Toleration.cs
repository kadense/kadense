namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Toleration : KadenseTemplatedCopiedResource<k8s.Models.V1Toleration>
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("operator")]
        public string? Operator { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("effect")]
        public string? Effect { get; set; }

        [JsonPropertyName("tolerationSeconds")]
        public long? TolerationSeconds { get; set; }

        public override k8s.Models.V1Toleration ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Toleration(
                key: this.GetValue(this.Key, variables),
                operatorProperty: this.GetValue(this.Operator, variables),
                value: this.GetValue(this.Value, variables),
                effect: this.GetValue(this.Value, variables),
                tolerationSeconds: this.TolerationSeconds
            );
        }
    }
}