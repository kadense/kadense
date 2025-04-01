using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CephFSVolumeSource: KadenseTemplatedCopiedResource<k8s.Models.V1CephFSVolumeSource>
    { 
        [JsonPropertyName("monitors")]
        public List<string>? Monitors { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("secretFile")]
        public string? SecretFile { get; set; }

        [JsonPropertyName("secretRef")]
        public V1LocalObjectReference? SecretRef { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1CephFSVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1CephFSVolumeSource(
                monitors: this.GetValue(this.Monitors, variables),
                path: this.GetValue(this.Path, variables),
                user: this.GetValue(this.User, variables),
                secretFile: this.GetValue(this.SecretFile, variables),
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}