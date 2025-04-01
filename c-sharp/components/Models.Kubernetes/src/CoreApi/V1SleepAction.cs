
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SleepAction : KadenseTemplatedCopiedResource<k8s.Models.V1SleepAction>
    {
        public long? Seconds { get; set; }

        public override k8s.Models.V1SleepAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SleepAction(
                seconds: this.Seconds!.Value
            );
        }
    }
}