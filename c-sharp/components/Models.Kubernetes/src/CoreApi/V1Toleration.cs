
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Toleration : KadenseTemplatedCopiedResource<k8s.Models.V1Toleration>
    {
        public string? Key { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public string? Effect { get; set; }
        public long? TolerationSeconds { get; set; }

        public override k8s.Models.V1Toleration ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Toleration(
                key: this.GetValue(this.Key, variables),
                operatorProperty: this.GetValue(this.Operator, variables),
                value: this.GetValue(this.Value, variables),
                effect: this.GetValue(this.Value, variables),
                tolerationSeconds: this.TolerationSeconds
            );
        }
    }
}