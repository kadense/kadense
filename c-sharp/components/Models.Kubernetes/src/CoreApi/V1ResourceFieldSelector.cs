namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ResourceFieldSelector : KadenseTemplatedCopiedResource<k8s.Models.V1ResourceFieldSelector>
    {
        [JsonPropertyName("containerName")]
        public string? ContainerName { get; set; }

        [JsonPropertyName("divisor")]
        public string? Divisor { get; set; }

        [JsonPropertyName("resource")]
        public string? Resource { get; set; }

        public override k8s.Models.V1ResourceFieldSelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ResourceFieldSelector(
                containerName: this.GetValue(this.ContainerName, variables),
                divisor: this.Divisor != null ? new k8s.Models.ResourceQuantity(this.GetValue(this.Divisor, variables)) : null,
                resource: this.GetValue(this.Resource, variables)
            );
        }
    }
}