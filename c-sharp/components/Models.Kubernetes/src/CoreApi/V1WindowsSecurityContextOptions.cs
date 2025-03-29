namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1WindowsSecurityContextOptions
    {
        public string? GMSACredentialSpec { get; set; }
        public string? GMSACredentialSpecName { get; set; }
        public string? HostProcess { get; set; }
        public string? RunAsUserName { get; set; }
    }
}