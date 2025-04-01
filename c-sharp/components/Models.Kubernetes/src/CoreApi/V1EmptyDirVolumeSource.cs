
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EmptyDirVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1EmptyDirVolumeSource>
    {
        public string? Medium { get; set; }
        public string? SizeLimit { get; set; }

        public override k8s.Models.V1EmptyDirVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EmptyDirVolumeSource(
                medium: this.GetValue(this.Medium, variables),
                sizeLimit: new k8s.Models.ResourceQuantity(this.GetValue(this.SizeLimit, variables))
            );
        }
    }
}