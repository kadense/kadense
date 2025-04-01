namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1RBDVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1RBDVolumeSource>
    {
        public string? FsType { get; set; }
        public string? Image { get; set; }
        public string? Keyring { get; set; }
        public List<string>? Monitors { get; set; }
        public string? Pool { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public string? User { get; set; }

        public override k8s.Models.V1RBDVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1RBDVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                keyring: this.GetValue(this.Keyring, variables),
                image: this.GetValue(this.Image, variables),
                monitors: this.GetValue(this.Monitors, variables),
                pool: this.GetValue(this.Pool, variables),
                readOnlyProperty: this.ReadOnly,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null,
                user: this.GetValue(this.User, variables)
            );
        }
    }
}