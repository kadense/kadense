namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecurityContext : KadenseTemplatedCopiedResource<k8s.Models.V1SecurityContext>
    {
        [JsonPropertyName("allowPrivilegeEscalation")]
        public bool? AllowPrivilegeEscalation { get; set; }

        [JsonPropertyName("appArmorProfile")]
        public V1AppArmorProfile? AppArmorProfile { get; set; }

        [JsonPropertyName("capabilities")]
        public V1Capabilities? Capabilities { get; set; }

        [JsonPropertyName("privileged")]
        public bool? Privileged { get; set; }

        [JsonPropertyName("procMount")]
        public string? ProcMount { get; set; }

        [JsonPropertyName("readOnlyRootFilesystem")]
        public bool? ReadOnlyRootFilesystem { get; set; }

        [JsonPropertyName("runAsGroup")]
        public long? RunAsGroup { get; set; }

        [JsonPropertyName("runAsNonRoot")]
        public bool? RunAsNonRoot { get; set; }

        [JsonPropertyName("runAsUser")]
        public long? RunAsUser { get; set; }

        [JsonPropertyName("seLinuxOptions")]
        public V1SELinuxOptions? SeLinuxOptions { get; set; }

        [JsonPropertyName("seccompProfile")]
        public V1SeccompProfile? SeccompProfile { get; set; }

        [JsonPropertyName("windowsOptions")]
        public V1WindowsSecurityContextOptions? WindowsOptions { get; set; }

        public override k8s.Models.V1SecurityContext ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SecurityContext(
                allowPrivilegeEscalation: this.AllowPrivilegeEscalation,
                appArmorProfile: this.AppArmorProfile != null ? this.AppArmorProfile.ToOriginal(variables) : null,
                capabilities: this.Capabilities != null ? this.Capabilities.ToOriginal(variables) : null,
                privileged: this.Privileged,
                procMount: this.GetValue(this.ProcMount, variables),
                readOnlyRootFilesystem: this.ReadOnlyRootFilesystem,
                runAsGroup: this.RunAsGroup,
                runAsNonRoot: this.RunAsNonRoot,
                runAsUser: this.RunAsUser,
                seccompProfile: this.SeccompProfile != null ? this.SeccompProfile.ToOriginal(variables) : null,
                seLinuxOptions: this.SeLinuxOptions != null ? this.SeLinuxOptions.ToOriginal(variables) : null,
                windowsOptions: this.WindowsOptions != null ? this.WindowsOptions.ToOriginal(variables) : null
            );
        }
    }
}