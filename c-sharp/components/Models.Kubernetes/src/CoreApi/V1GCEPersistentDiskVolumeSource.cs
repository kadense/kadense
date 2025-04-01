
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GCEPersistentDiskVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1GCEPersistentDiskVolumeSource>
    {
        public string? PdName { get; set; }
        public string? FsType { get; set; }
        public int? Partition { get; set; }
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1GCEPersistentDiskVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1GCEPersistentDiskVolumeSource(
                pdName: this.GetValue(this.PdName, variables),
                fsType: this.GetValue(this.FsType, variables),
                partition: this.Partition,
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}