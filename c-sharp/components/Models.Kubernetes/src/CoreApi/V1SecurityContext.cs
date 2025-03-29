namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecurityContext
    {
        public bool? AllowPrivilegeEscalation { get; set; }
        public V1AppArmorProfile? AppArmorProfile { get; set; }
        public V1Capabilities? Capabilities { get; set; }
        public bool? Privileged { get; set; }
        public string? ProcMount { get; set; }
        public bool? ReadOnlyRootFilesystem { get; set; }
        public int? RunAsGroup { get; set; }
        public bool? RunAsNonRoot { get; set; }
        public int? RunAsUser { get; set; }
        public V1SELinuxOptions? SeLinuxOptions { get; set; }
        public V1SeccompProfile? SeccompProfile { get; set; }
        public V1WindowsSecurityContextOptions? WindowsOptions { get; set; }
    }
}