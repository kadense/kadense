
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodOS : KadenseTemplatedCopiedResource<k8s.Models.V1PodOS>
    {
        public string? Name { get; set; }

        public override k8s.Models.V1PodOS ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodOS(
                name: this.GetValue(this.Name, variables)
            );
        }
    }
}