
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1OwnerReference : KadenseTemplatedCopiedResource<k8s.Models.V1OwnerReference>
    {
        public string? ApiVersion { get; set; }
        public string? Kind { get; set; }
        public string? Name { get; set; }
        public string? Uid { get; set; }
        public bool? Controller { get; set; }
        public bool? BlockOwnerDeletion { get; set; }

        public override k8s.Models.V1OwnerReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1OwnerReference(
                apiVersion: this.GetValue(this.ApiVersion, variables),
                kind: this.GetValue(this.Kind, variables),
                name: this.GetValue(this.Name, variables),
                uid: this.GetValue(this.Uid, variables),
                blockOwnerDeletion: this.BlockOwnerDeletion,
                controller: this.Controller
            );
        }
    }
}