using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureFileVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1AzureFileVolumeSource>
    { 
        [JsonPropertyName("secretName")]
        public string? SecretName { get; set; }

        [JsonPropertyName("shareName")]
        public string? ShareName { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1AzureFileVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1AzureFileVolumeSource(
                secretName: this.GetValue(this.SecretName, variables),
                shareName: this.GetValue(this.ShareName, variables),
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}