namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PodSpec
    {
        public int? ActiveDeadlineSeconds { get; set; }

        public V1Affinity? Affinity { get; set; }

        public bool? AutomountServiceAccountToken { get; set; }

        public List<V1Container>? Containers { get; set; }

        public List<V1Volume>? Volumes { get; set; }
        public List<V1PodDNSConfig>? DnsConfig { get; set; }
        public List<string>? DnsPolicy { get; set; }

        public bool? EnableServiceLinks { get; set; }

        public List<V1EphemeralContainer>? EphemeralContainers { get; set; }

        public List<V1HostAlias>? HostAliases { get; set; }

        public bool? HostIPC { get; set; }

        public bool? HostNetwork { get; set; }

        public bool? HostPID { get; set; }

        public bool? HostUsers { get; set; }

        public string? HostName { get; set; }

        public List<V1LocalObjectReference>? ImagePullSecrets { get; set; }

        public List<V1Container>? InitContainers { get; set; }

        public string? NodeName { get; set; }

        public Dictionary<string, string>? NodeSelector { get; set; }

        public V1PodOS? Os { get; set; }

        public Dictionary<string, string>? Overhead { get; set; }

        public string? PreemptionPolicy { get; set; }

        public int? Priority { get; set; }

        public string? PriorityClassName { get; set; }

        public List<V1PodReadinessGate>? ReadinessGates { get; set; }

        public List<V1PodResourceClaim>? ResourceClaims { get; set; }

        public V1ResourceRequirements? Resources { get; set; }

        public string? RestartPolicy { get; set; }

        public string? RuntimeClassName { get; set; }

        public string? SchedulerName { get; set; }

        public List<V1PodSchedulingGate>? SchedulingGates { get; set; }

        public V1PodSecurityContext? SecurityContext { get; set; }

        public string? ServiceAccount { get; set; }
        public string? ServiceAccountName { get; set; }

        public bool? SetHostnameAsFQDN { get; set; }

        public bool? ShareProcessNamespace { get; set; }

        public string? Subdomain { get; set; }

        public int? TerminationGracePeriodSeconds { get; set; }

        public List<V1Toleration>? Tolerations { get; set; }

        public List<V1TopologySpreadConstraint>? TopologySpreadConstraints { get; set; }

        public List<V1VolumeMount>? VolumeMounts { get; set; }        
    }
}