namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CephFSVolumeSource: KadenseTemplatedCopiedResource<k8s.Models.V1CephFSVolumeSource>
    { 
        public List<string>? Monitors { get; set; }
        public string? Path { get; set; }
        public string? User { get; set; }
        public string? SecretFile { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1CephFSVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1CephFSVolumeSource(
                monitors: this.GetValue(this.Monitors, variables),
                path: this.GetValue(this.Path, variables),
                user: this.GetValue(this.User, variables),
                secretFile: this.GetValue(this.SecretFile, variables),
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}