namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1KeyToPath : KadenseTemplatedCopiedResource<k8s.Models.V1KeyToPath>
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("mode")]
        public int? Mode { get; set; }

        public override k8s.Models.V1KeyToPath ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1KeyToPath(
                key: this.GetValue(this.Key, variables),
                path: this.GetValue(this.Path, variables),
                mode: this.Mode
            );
        }
    }
}