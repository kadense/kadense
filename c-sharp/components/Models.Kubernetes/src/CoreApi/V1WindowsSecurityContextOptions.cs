
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1WindowsSecurityContextOptions : KadenseTemplatedCopiedResource<k8s.Models.V1WindowsSecurityContextOptions>
    {
        public string? GMSACredentialSpec { get; set; }
        public string? GMSACredentialSpecName { get; set; }
        public bool? HostProcess { get; set; }
        public string? RunAsUserName { get; set; }

        public override k8s.Models.V1WindowsSecurityContextOptions ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1WindowsSecurityContextOptions(
                gmsaCredentialSpec: this.GetValue(this.GMSACredentialSpec, variables),
                gmsaCredentialSpecName: this.GetValue(this.GMSACredentialSpecName, variables),
                hostProcess: this.HostProcess,
                runAsUserName: this.GetValue(this.RunAsUserName, variables)
            );
        }
    }
}