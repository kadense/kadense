namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PersistentVolumeClaimSpec : KadenseTemplatedCopiedResource<k8s.Models.V1PersistentVolumeClaimSpec>
    {
        [JsonPropertyName("accessModes")]
        public List<string>? AccessModes { get; set; }

        [JsonPropertyName("dataSource")]
        public V1TypedLocalObjectReference? DataSource { get; set; }

        [JsonPropertyName("dataSourceRef")]
        public V1TypedObjectReference? DataSourceRef { get; set; }

        [JsonPropertyName("resources")]
        public V1VolumeResourceRequirements? Resources { get; set; }

        [JsonPropertyName("selector")]
        public V1LabelSelector? Selector { get; set; }

        [JsonPropertyName("storageClassName")]
        public string? StorageClassName { get; set; }

        [JsonPropertyName("volumeAttributesClassName")]
        public string? VolumeAttributesClassName { get; set; }

        [JsonPropertyName("volumeMode")]
        public string? VolumeMode { get; set; }

        [JsonPropertyName("volumeName")]
        public string? VolumeName { get; set; }

        public override k8s.Models.V1PersistentVolumeClaimSpec ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1PersistentVolumeClaimSpec(
                accessModes: this.GetValue(this.AccessModes, variables),
                dataSource: this.DataSource != null ? this.DataSource.ToOriginal(variables) : null,
                dataSourceRef: this.DataSourceRef != null ? this.DataSourceRef.ToOriginal(variables) : null,
                resources: this.Resources != null ? this.Resources.ToOriginal(variables) : null,
                selector: this.Selector != null ? this.Selector.ToOriginal(variables) : null,
                storageClassName: this.GetValue(this.StorageClassName, variables),
                volumeAttributesClassName: this.GetValue(this.VolumeAttributesClassName, variables),
                volumeMode: this.GetValue(this.VolumeMode, variables),
                volumeName: this.GetValue(this.VolumeName, variables)
            );
        }
    }
}