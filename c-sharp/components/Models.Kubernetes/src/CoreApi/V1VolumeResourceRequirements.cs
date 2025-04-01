using YamlDotNet.Core.Tokens;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeResourceRequirements : KadenseTemplatedCopiedResource<k8s.Models.V1VolumeResourceRequirements>
    {
        [JsonPropertyName("requests")]
        public Dictionary<string, string>? Requests { get; set; }

        [JsonPropertyName("limits")]
        public Dictionary<string, string>? Limits { get; set; }

        public override k8s.Models.V1VolumeResourceRequirements ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1VolumeResourceRequirements(
                requests: this.GetValueAsResourceQuantity(this.Requests, variables),
                limits: this.GetValueAsResourceQuantity(this.Limits, variables)
            );
        }
    }
}