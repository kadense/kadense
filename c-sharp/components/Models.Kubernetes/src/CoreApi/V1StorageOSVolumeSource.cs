
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1StorageOSVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1StorageOSVolumeSource>
    {
        public string? VolumeName { get; set; }
        public string? VolumeNamespace { get; set; }
        public bool? ReadOnly { get; set; }
        public string? FsType { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }

        public override k8s.Models.V1StorageOSVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1StorageOSVolumeSource(
                volumeName: this.GetValue(this.VolumeName, variables),
                volumeNamespace: this.GetValue(this.VolumeNamespace, variables),
                readOnlyProperty: this.ReadOnly,
                fsType: this.GetValue(this.FsType, variables),
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null
            );
        }
    }
}