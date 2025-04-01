namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LocalObjectReference : KadenseTemplatedCopiedResource<k8s.Models.V1LocalObjectReference>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public override k8s.Models.V1LocalObjectReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1LocalObjectReference(
                name: this.GetValue(this.Name, variables)
            );
        }
    }
}