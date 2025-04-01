namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1DownwardAPIVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1DownwardAPIVolumeSource>
    {
        [JsonPropertyName("defaultMode")]
        public int? DefaultMode { get; set; }

        [JsonPropertyName("items")]
        public List<V1DownwardAPIVolumeFile>? Items { get; set; }

        public override k8s.Models.V1DownwardAPIVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1DownwardAPIVolumeSource(
                defaultMode: this.DefaultMode,
                items: this.GetValue<V1DownwardAPIVolumeFile, k8s.Models.V1DownwardAPIVolumeFile>(this.Items, variables)
            );
        }
    }
}