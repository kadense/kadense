namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ResourceClaim : KadenseTemplatedCopiedResource<k8s.Models.V1ResourceClaim>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("request")]
        public string? Request { get; set; }

        public override k8s.Models.V1ResourceClaim ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ResourceClaim(
                name: this.GetValue(this.Name, variables),
                request: this.GetValue(this.Request, variables)
            );
        }
    }
}