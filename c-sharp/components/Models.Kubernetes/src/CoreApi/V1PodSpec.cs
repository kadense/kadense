namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSpec : KadenseTemplatedCopiedResource<k8s.Models.V1PodSpec>
    {
        [JsonPropertyName("activeDeadlineSeconds")]
        public long? ActiveDeadlineSeconds { get; set; }

        [JsonPropertyName("affinity")]
        public V1Affinity? Affinity { get; set; }

        [JsonPropertyName("automountServiceAccountToken")]
        public bool? AutomountServiceAccountToken { get; set; }

        [JsonPropertyName("containers")]
        public List<V1Container>? Containers { get; set; }

        [JsonPropertyName("volumes")]
        public List<V1Volume>? Volumes { get; set; }

        [JsonPropertyName("dnsConfig")]
        public V1PodDNSConfig? DnsConfig { get; set; }

        [JsonPropertyName("dnsPolicy")]
        public string? DnsPolicy { get; set; }

        [JsonPropertyName("enableServiceLinks")]
        public bool? EnableServiceLinks { get; set; }

        [JsonPropertyName("ephemeralContainers")]
        public List<V1EphemeralContainer>? EphemeralContainers { get; set; }

        [JsonPropertyName("hostAliases")]
        public List<V1HostAlias>? HostAliases { get; set; }

        [JsonPropertyName("hostIPC")]
        public bool? HostIPC { get; set; }

        [JsonPropertyName("hostNetwork")]
        public bool? HostNetwork { get; set; }

        [JsonPropertyName("hostPID")]
        public bool? HostPID { get; set; }

        [JsonPropertyName("hostUsers")]
        public bool? HostUsers { get; set; }

        [JsonPropertyName("hostname")]
        public string? HostName { get; set; }

        [JsonPropertyName("imagePullSecrets")]
        public List<V1LocalObjectReference>? ImagePullSecrets { get; set; }

        [JsonPropertyName("initContainers")]
        public List<V1Container>? InitContainers { get; set; }

        [JsonPropertyName("nodeName")]
        public string? NodeName { get; set; }

        [JsonPropertyName("nodeSelector")]
        public Dictionary<string, string>? NodeSelector { get; set; }

        [JsonPropertyName("os")]
        public V1PodOS? Os { get; set; }

        [JsonPropertyName("overhead")]
        public Dictionary<string, string>? Overhead { get; set; }

        [JsonPropertyName("preemptionPolicy")]
        public string? PreemptionPolicy { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("priorityClassName")]
        public string? PriorityClassName { get; set; }

        [JsonPropertyName("readinessGates")]
        public List<V1PodReadinessGate>? ReadinessGates { get; set; }

        [JsonPropertyName("resourceClaims")]
        public List<V1PodResourceClaim>? ResourceClaims { get; set; }

        [JsonPropertyName("resources")]
        public V1ResourceRequirements? Resources { get; set; }

        [JsonPropertyName("restartPolicy")]
        public string? RestartPolicy { get; set; }

        [JsonPropertyName("runtimeClassName")]
        public string? RuntimeClassName { get; set; }

        [JsonPropertyName("schedulerName")]
        public string? SchedulerName { get; set; }

        [JsonPropertyName("schedulingGates")]
        public List<V1PodSchedulingGate>? SchedulingGates { get; set; }

        [JsonPropertyName("securityContext")]
        public V1PodSecurityContext? SecurityContext { get; set; }

        [JsonPropertyName("serviceAccount")]
        public string? ServiceAccount { get; set; }

        [JsonPropertyName("serviceAccountName")]
        public string? ServiceAccountName { get; set; }

        [JsonPropertyName("setHostnameAsFQDN")]
        public bool? SetHostnameAsFQDN { get; set; }

        [JsonPropertyName("shareProcessNamespace")]
        public bool? ShareProcessNamespace { get; set; }

        [JsonPropertyName("subdomain")]
        public string? Subdomain { get; set; }

        [JsonPropertyName("terminationGracePeriodSeconds")]
        public long? TerminationGracePeriodSeconds { get; set; }

        [JsonPropertyName("tolerations")]
        public List<V1Toleration>? Tolerations { get; set; }

        [JsonPropertyName("topologySpreadConstraints")]
        public List<V1TopologySpreadConstraint>? TopologySpreadConstraints { get; set; }

        public override k8s.Models.V1PodSpec ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PodSpec(
                containers: this.GetValue<V1Container, k8s.Models.V1Container>(this.Containers, variables),
                activeDeadlineSeconds: this.ActiveDeadlineSeconds,
                affinity: this.Affinity != null ? this.Affinity.ToOriginal(variables) : null,
                automountServiceAccountToken: this.AutomountServiceAccountToken,
                volumes: this.GetValue<V1Volume, k8s.Models.V1Volume>(this.Volumes, variables),
                dnsConfig: this.DnsConfig != null ? this.DnsConfig.ToOriginal(variables) : null,
                dnsPolicy: this.GetValue(this.DnsPolicy, variables),
                enableServiceLinks: this.EnableServiceLinks,
                ephemeralContainers: this.GetValue<V1EphemeralContainer, k8s.Models.V1EphemeralContainer>(this.EphemeralContainers, variables),
                hostAliases: this.GetValue<V1HostAlias, k8s.Models.V1HostAlias>(this.HostAliases, variables),
                hostIPC: this.HostIPC,
                hostNetwork: this.HostNetwork,
                hostPID: this.HostPID,
                hostUsers: this.HostUsers,
                hostname: this.HostName,
                imagePullSecrets: this.GetValue<V1LocalObjectReference, k8s.Models.V1LocalObjectReference>(this.ImagePullSecrets, variables),
                initContainers: this.GetValue<V1Container, k8s.Models.V1Container>(this.InitContainers, variables),
                nodeName: this.GetValue(this.NodeName, variables),
                nodeSelector: this.GetValue(this.NodeSelector, variables),
                os: this.Os != null ? this.Os.ToOriginal(variables) : null,
                overhead: this.GetValueAsResourceQuantity(this.Overhead, variables),
                preemptionPolicy: this.GetValue(this.PreemptionPolicy, variables),
                priority: this.Priority,
                priorityClassName: this.GetValue(this.PriorityClassName, variables),
                readinessGates: this.GetValue<V1PodReadinessGate, k8s.Models.V1PodReadinessGate>(this.ReadinessGates, variables),
                resourceClaims: this.GetValue<V1PodResourceClaim, k8s.Models.V1PodResourceClaim>(this.ResourceClaims, variables),
                resources: this.Resources != null ? this.Resources.ToOriginal(variables) : null,
                restartPolicy: this.GetValue(this.RestartPolicy, variables),
                runtimeClassName: this.GetValue(this.RuntimeClassName, variables),
                schedulerName: this.GetValue(this.SchedulerName, variables),
                schedulingGates: this.GetValue<V1PodSchedulingGate, k8s.Models.V1PodSchedulingGate>(this.SchedulingGates, variables),
                securityContext: this.SecurityContext != null ? this.SecurityContext.ToOriginal(variables) : null,
                serviceAccount: this.GetValue(this.ServiceAccount, variables),
                serviceAccountName: this.GetValue(this.ServiceAccountName, variables),
                setHostnameAsFQDN: this.SetHostnameAsFQDN,
                shareProcessNamespace: this.ShareProcessNamespace,
                subdomain: this.GetValue(this.Subdomain, variables),
                terminationGracePeriodSeconds: this.TerminationGracePeriodSeconds,
                tolerations: this.GetValue<V1Toleration, k8s.Models.V1Toleration>(this.Tolerations, variables),
                topologySpreadConstraints: this.GetValue<V1TopologySpreadConstraint, k8s.Models.V1TopologySpreadConstraint>(this.TopologySpreadConstraints, variables)
            );
        }
    }
}