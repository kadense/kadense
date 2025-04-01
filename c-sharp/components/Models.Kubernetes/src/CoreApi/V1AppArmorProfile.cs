namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AppArmorProfile : KadenseTemplatedCopiedResource<k8s.Models.V1AppArmorProfile>
    { 
        public string? LocalhostProfile { get; set; }
        public string? Type { get; set; }

        public override k8s.Models.V1AppArmorProfile ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1AppArmorProfile(
                this.GetValue(this.Type, variables),
                this.GetValue(this.LocalhostProfile, variables)
            );
        }
    }
}