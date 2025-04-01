
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1FCVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1FCVolumeSource>
    {
        public string? FsType { get; set; }
        public int? Lun { get; set; }
        public bool? ReadOnly { get; set; }
        public List<string>? TargetWWNS { get; set; }
        public List<string>? Wwids { get; set; }

        public override k8s.Models.V1FCVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1FCVolumeSource(
                fsType: this.GetValue(this.FsType, variables),
                lun: this.Lun,
                readOnlyProperty: this.ReadOnly,
                this.GetValue(this.TargetWWNS, variables),
                this.GetValue(this.Wwids, variables)
            );
        }
    }
}