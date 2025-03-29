namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodResourceClaim
    {
        public string? Name { get; set; }
        public string? ResourceClaimName { get; set; }
        public string? ResourceClaimTemplateName { get; set; }
    }
}