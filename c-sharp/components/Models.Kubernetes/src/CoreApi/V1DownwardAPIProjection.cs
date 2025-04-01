namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1DownwardAPIProjection : KadenseTemplatedCopiedResource<k8s.Models.V1DownwardAPIProjection>
    {
        public List<V1DownwardAPIVolumeFile>? Items { get; set; }

        public override k8s.Models.V1DownwardAPIProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1DownwardAPIProjection(
                items: this.GetValue<V1DownwardAPIVolumeFile, k8s.Models.V1DownwardAPIVolumeFile>(this.Items, variables)
            );
        }
    }
}