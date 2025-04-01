
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PhotonPersistentDiskVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1PhotonPersistentDiskVolumeSource>
    {
        public string? PdID { get; set; }
        public string? FsType { get; set; }

        public override k8s.Models.V1PhotonPersistentDiskVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PhotonPersistentDiskVolumeSource(
                pdID: this.GetValue(this.PdID, variables),
                fsType: this.GetValue(this.FsType, variables)
            );
        }
    }
}