namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1FlexVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1FlexVolumeSource>
    {
        public string? Driver { get; set; }
        public Dictionary<string, string>? Options { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }
        public string? FsType { get; set; }

        public override k8s.Models.V1FlexVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1FlexVolumeSource(
                driver: this.GetValue(this.Driver, variables),
                options: this.GetValue(this.Options, variables),
                readOnlyProperty: this.ReadOnly,
                fsType: this.GetValue(this.FsType, variables),
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null
            );
        }
    }
}