namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSecurityContext
    {
        public V1AppArmorProfile? AppArmorProfile { get; set; }
        public V1SeccompProfile? SeccompProfile { get; set; }
        public V1WindowsSecurityContextOptions? WindowsOptions { get; set; }
        public List<V1Sysctl>? Sysctls { get; set; }
        public int? FsGroup { get; set; }
        public string? FsGroupChangePolicy { get; set; }
        public int? RunAsGroup { get; set; }
        public bool? RunAsNonRoot { get; set; }
        public int? RunAsUser { get; set; }
        public V1SELinuxOptions? SeLinuxOptions { get; set; }
        public List<int>? SupplementalGroups { get; set; }
        public string? SupplementalGroupsPolicy { get; set; }
    }
}