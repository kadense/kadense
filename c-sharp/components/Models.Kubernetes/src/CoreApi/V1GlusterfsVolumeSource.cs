
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1GlusterfsVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1GlusterfsVolumeSource>
    {
        public string? Endpoints { get; set; }
        public string? Path { get; set; }
        public bool? ReadOnly { get; set; }

        public override k8s.Models.V1GlusterfsVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1GlusterfsVolumeSource(
                endpoints: this.GetValue(this.Endpoints, variables),
                path: this.GetValue(this.Path, variables),
                readOnlyProperty: this.ReadOnly
            );
        }
    }
}