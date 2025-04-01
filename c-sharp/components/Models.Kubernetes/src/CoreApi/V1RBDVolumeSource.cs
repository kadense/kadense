namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1RBDVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1RBDVolumeSource>
    {
        [JsonPropertyName("fsType")]
        public string? FsType { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("keyring")]
        public string? Keyring { get; set; }

        [JsonPropertyName("monitors")]
        public List<string>? Monitors { get; set; }

        [JsonPropertyName("pool")]
        public string? Pool { get; set; }

        [JsonPropertyName("readOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("secretRef")]
        public V1LocalObjectReference? SecretRef { get; set; }

        [JsonPropertyName("user")]
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