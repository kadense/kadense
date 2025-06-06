namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1DownwardAPIVolumeFile : KadenseTemplatedCopiedResource<k8s.Models.V1DownwardAPIVolumeFile>
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("fieldRef")]
        public V1ObjectFieldSelector? FieldRef { get; set; }

        [JsonPropertyName("resourceFieldRef")]
        public V1ResourceFieldSelector? ResourceFieldRef { get; set; }

        public override k8s.Models.V1DownwardAPIVolumeFile ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1DownwardAPIVolumeFile(
                path: this.GetValue(this.Path, variables),
                fieldRef: this.FieldRef != null ? this.FieldRef.ToOriginal(variables) : null,
                resourceFieldRef: this.ResourceFieldRef != null ? this.ResourceFieldRef.ToOriginal(variables) : null
            );
        }
    }
}