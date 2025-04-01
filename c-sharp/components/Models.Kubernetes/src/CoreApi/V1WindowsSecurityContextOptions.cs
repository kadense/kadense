namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1WindowsSecurityContextOptions : KadenseTemplatedCopiedResource<k8s.Models.V1WindowsSecurityContextOptions>
    {
        [JsonPropertyName("gmsaCredentialSpec")]
        public string? GMSACredentialSpec { get; set; }

        [JsonPropertyName("gmsaCredentialSpecName")]
        public string? GMSACredentialSpecName { get; set; }

        [JsonPropertyName("hostProcess")]
        public bool? HostProcess { get; set; }

        [JsonPropertyName("runAsUserName")]
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