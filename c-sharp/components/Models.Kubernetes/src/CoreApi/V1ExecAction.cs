
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ExecAction : KadenseTemplatedCopiedResource<k8s.Models.V1ExecAction>
    {
        public List<string>? Command { get; set; }

        public override k8s.Models.V1ExecAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ExecAction(
                command: this.GetValue(this.Command, variables)
            );
        }
    }
}