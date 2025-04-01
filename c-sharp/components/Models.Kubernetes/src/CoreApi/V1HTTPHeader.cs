
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HTTPHeader : KadenseTemplatedCopiedResource<k8s.Models.V1HTTPHeader>
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        public override k8s.Models.V1HTTPHeader ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1HTTPHeader(
                name: this.GetValue(this.Name, variables),
                value: this.GetValue(this.Value, variables)
            );
        }
    }
}