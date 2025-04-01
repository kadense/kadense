
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1NodeSelectorRequirement : KadenseTemplatedCopiedResource<k8s.Models.V1NodeSelectorRequirement>
    {
        public string? Key { get; set; }
        public string? Operator { get; set; }
        public List<string>? Values { get; set; }

        public override k8s.Models.V1NodeSelectorRequirement ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1NodeSelectorRequirement(
                key: this.GetValue(this.Key, variables),
                operatorProperty: this.GetValue(this.Operator, variables),
                values: this.GetValue(this.Values, variables)
            );
        }
    }
}