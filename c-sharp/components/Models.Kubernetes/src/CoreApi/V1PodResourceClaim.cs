
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodResourceClaim : KadenseTemplatedCopiedResource<k8s.Models.V1PodResourceClaim>
    {
        public string? Name { get; set; }
        public string? ResourceClaimName { get; set; }
        public string? ResourceClaimTemplateName { get; set; }

        public override k8s.Models.V1PodResourceClaim ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodResourceClaim(
                name: this.GetValue(this.Name, variables),
                resourceClaimName: this.GetValue(this.ResourceClaimName, variables),
                resourceClaimTemplateName: this.GetValue(this.ResourceClaimTemplateName, variables)
            );
        }
    }
}