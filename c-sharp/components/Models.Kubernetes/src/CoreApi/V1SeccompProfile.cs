
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SeccompProfile : KadenseTemplatedCopiedResource<k8s.Models.V1SeccompProfile>
    {
        public string? Type { get; set; }
        public string? LocalhostProfile { get; set; }

        public override k8s.Models.V1SeccompProfile ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SeccompProfile(
                type: this.GetValue(this.Type, variables),
                localhostProfile: this.GetValue(this.LocalhostProfile, variables)
            );
        }
    }
}