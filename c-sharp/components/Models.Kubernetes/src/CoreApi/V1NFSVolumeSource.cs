namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NFSVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1NFSVolumeSource>
    {
        [JsonPropertyName("server")]
        public string? Server { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1NFSVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1NFSVolumeSource(
                server: this.GetValue(this.Server, variables),
                path: this.GetValue(this.Path, variables),
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}