
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PortworxVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1PortworxVolumeSource>
    {
        public string? FsType { get; set; }
        public string? VolumeID { get; set; }
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1PortworxVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PortworxVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                volumeID: this.GetValue(this.VolumeID, variables),
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}