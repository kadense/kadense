namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CSIVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1CSIVolumeSource>
    {
        public string? Driver { get; set; }
        public string? FsType { get; set; }
        public V1LocalObjectReference? NodePublishSecretRef { get; set; }
        public bool? ReadOnly { get; set; }
        public Dictionary<string, string>? VolumeAttributes { get; set; }

        public override k8s.Models.V1CSIVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1CSIVolumeSource(
                driver: this.GetValue(this.Driver, variables),
                fsType: this.GetValue(this.FsType, variables),
                nodePublishSecretRef: this.NodePublishSecretRef != null ? this.NodePublishSecretRef.ToOriginal(variables) : null,
                readOnlyProperty: this.ReadOnly,
                volumeAttributes: this.GetValue(this.VolumeAttributes, variables)
            );
        }
    }
}