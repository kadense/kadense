namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSchedulingGate : KadenseTemplatedCopiedResource<k8s.Models.V1PodSchedulingGate>
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public override k8s.Models.V1PodSchedulingGate ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodSchedulingGate(
                name: this.GetValue(this.Name, variables)
            );
        }
    }
}