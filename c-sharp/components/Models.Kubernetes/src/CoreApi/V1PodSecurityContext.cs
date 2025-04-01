namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSecurityContext : KadenseTemplatedCopiedResource<k8s.Models.V1PodSecurityContext>
    {
        public V1AppArmorProfile? AppArmorProfile { get; set; }
        public V1SeccompProfile? SeccompProfile { get; set; }
        public V1WindowsSecurityContextOptions? WindowsOptions { get; set; }
        public List<V1Sysctl>? Sysctls { get; set; }
        public long? FsGroup { get; set; }
        public string? FsGroupChangePolicy { get; set; }
        public long? RunAsGroup { get; set; }
        public bool? RunAsNonRoot { get; set; }
        public long? RunAsUser { get; set; }
        public V1SELinuxOptions? SeLinuxOptions { get; set; }
        public List<long?>? SupplementalGroups { get; set; }
        public string? SupplementalGroupsPolicy { get; set; }

        public override k8s.Models.V1PodSecurityContext ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodSecurityContext(
                appArmorProfile: this.AppArmorProfile != null ? this.AppArmorProfile.ToOriginal(variables) : null,
                fsGroup: this.FsGroup,
                fsGroupChangePolicy: this.GetValue(this.FsGroupChangePolicy, variables),
                runAsGroup: this.RunAsGroup,
                runAsNonRoot: this.RunAsNonRoot,
                runAsUser: this.RunAsUser,
                seccompProfile: this.SeccompProfile != null ? this.SeccompProfile.ToOriginal(variables) : null,
                seLinuxOptions: this.SeLinuxOptions != null ? this.SeLinuxOptions.ToOriginal(variables) : null,
                supplementalGroups: this.SupplementalGroups,
                supplementalGroupsPolicy: this.GetValue(this.SupplementalGroupsPolicy, variables),
                sysctls: this.GetValue<V1Sysctl, k8s.Models.V1Sysctl>(this.Sysctls, variables),
                windowsOptions: this.WindowsOptions != null ? this.WindowsOptions.ToOriginal(variables) : null
            );
        }
    }
}