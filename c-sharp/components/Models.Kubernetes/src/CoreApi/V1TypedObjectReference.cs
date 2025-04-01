
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TypedObjectReference : KadenseTemplatedCopiedResource<k8s.Models.V1TypedObjectReference>
    {
        public string? ApiGroup { get; set; }
        public string? Kind { get; set; }
        public string? Name { get; set; }
        public string? Namespace { get; set; }

        public override k8s.Models.V1TypedObjectReference ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1TypedObjectReference(
                namespaceProperty: this.GetValue(this.Namespace, variables),
                kind: this.GetValue(this.Kind, variables),
                name: this.GetValue(this.Name, variables),
                apiGroup: this.GetValue(this.ApiGroup, variables)
            );
        }
    }
}