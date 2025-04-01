
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureFileVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1AzureFileVolumeSource>
    { 
        public string? SecretName { get; set; }
        public string? ShareName { get; set; }
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