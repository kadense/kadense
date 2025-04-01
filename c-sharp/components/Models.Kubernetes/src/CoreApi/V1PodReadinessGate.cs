
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodReadinessGate : KadenseTemplatedCopiedResource<k8s.Models.V1PodReadinessGate>
    {
        public string? ConditionType { get; set; }

        public override k8s.Models.V1PodReadinessGate ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodReadinessGate(
                conditionType: this.GetValue(this.ConditionType, variables)
            );
        }
    }
}