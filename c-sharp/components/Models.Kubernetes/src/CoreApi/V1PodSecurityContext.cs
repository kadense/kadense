namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSecurityContext : KadenseTemplatedCopiedResource<k8s.Models.V1PodSecurityContext>
    {
        [JsonPropertyName("appArmorProfile")]
        public V1AppArmorProfile? AppArmorProfile { get; set; }

        [JsonPropertyName("seccompProfile")]
        public V1SeccompProfile? SeccompProfile { get; set; }

        [JsonPropertyName("windowsOptions")]
        public V1WindowsSecurityContextOptions? WindowsOptions { get; set; }

        [JsonPropertyName("sysctls")]
        public List<V1Sysctl>? Sysctls { get; set; }

        [JsonPropertyName("fsGroup")]
        public long? FsGroup { get; set; }

        [JsonPropertyName("fsGroupChangePolicy")]
        public string? FsGroupChangePolicy { get; set; }

        [JsonPropertyName("runAsGroup")]
        public long? RunAsGroup { get; set; }

        [JsonPropertyName("runAsNonRoot")]
        public bool? RunAsNonRoot { get; set; }

        [JsonPropertyName("runAsUser")]
        public long? RunAsUser { get; set; }

        [JsonPropertyName("seLinuxOptions")]
        public V1SELinuxOptions? SeLinuxOptions { get; set; }

        [JsonPropertyName("supplementalGroups")]
        public List<long?>? SupplementalGroups { get; set; }

        [JsonPropertyName("supplementalGroupsPolicy")]
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