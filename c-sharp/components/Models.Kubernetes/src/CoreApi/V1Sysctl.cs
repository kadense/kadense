
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Sysctl : KadenseTemplatedCopiedResource<k8s.Models.V1Sysctl>
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        public override k8s.Models.V1Sysctl ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Sysctl(
                name: this.GetValue(this.Name, variables),
                value: this.GetValue(this.Value, variables)
            );
        }
    }
}