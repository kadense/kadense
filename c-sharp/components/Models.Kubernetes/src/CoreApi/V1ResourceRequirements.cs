namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ResourceRequirements : KadenseTemplatedCopiedResource<k8s.Models.V1ResourceRequirements>
    {
        public List<V1ResourceClaim>? Claims { get; set; }
        public Dictionary<string, string>? Limits { get; set; }
        public Dictionary<string, string>? Requests { get; set; }

        public override k8s.Models.V1ResourceRequirements ToOriginal(Dictionary<string, string> variables)
        {
            var limits = new Dictionary<string, k8s.Models.ResourceQuantity>();
            

            return new k8s.Models.V1ResourceRequirements(
                claims: this.GetValue<V1ResourceClaim, k8s.Models.V1ResourceClaim>(this.Claims, variables),
                limits: this.GetValueAsResourceQuantity(this.Limits, variables),
                requests: this.GetValueAsResourceQuantity(this.Requests, variables)
            );
        }
    }
}