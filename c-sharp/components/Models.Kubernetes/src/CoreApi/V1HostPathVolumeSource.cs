namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HostPathVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1HostPathVolumeSource>
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        public override k8s.Models.V1HostPathVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1HostPathVolumeSource(
                path: this.GetValue(this.Path, variables),
                type: this.GetValue(this.Type, variables)
            );
        }
    }
}