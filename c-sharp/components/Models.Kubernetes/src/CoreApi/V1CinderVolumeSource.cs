
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1CinderVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1CinderVolumeSource>
    { 
        public string? FsType { get; set; }
        public string? VolumeID { get; set; }
        public bool? ReadOnly { get; set; }
        public V1LocalObjectReference? SecretRef { get; set; }

        public override k8s.Models.V1CinderVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1CinderVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                volumeID: this.GetValue(this.VolumeID, variables),
                readOnlyProperty: this.ReadOnly,
                secretRef: this.SecretRef != null ? this.SecretRef.ToOriginal(variables) : null
            );
        }
    }
}