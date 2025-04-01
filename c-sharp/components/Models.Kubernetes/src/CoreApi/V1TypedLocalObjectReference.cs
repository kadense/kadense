
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TypedLocalObjectReference : KadenseTemplatedCopiedResource<k8s.Models.V1TypedLocalObjectReference>
    {
        public string? Kind { get; set; }
        public string? Name { get; set; }
        public string? ApiGroup { get; set; }

        public override k8s.Models.V1TypedLocalObjectReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1TypedLocalObjectReference(
                kind: this.GetValue(this.Kind, variables),
                name: this.GetValue(this.Name, variables),
                apiGroup: this.GetValue(this.ApiGroup, variables)
            );
        }
    }
}