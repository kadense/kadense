namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PersistentVolumeClaimSpec
    {
        public List<string>? AccessModes { get; set; }
        public V1TypedLocalObjectReference? DataSource { get; set; }
        public V1TypedObjectReference? DataSourceRef { get; set; }
        public V1VolumeResourceRequirements? Resources { get; set; }
        public V1LabelSelector? Selector { get; set; }
        public string? StorageClassName { get; set; }
        public string? VolumeAttributesClassName { get; set; }
        public string? VolumeMode { get; set; }
        public string? VolumeName { get; set; }
    }
}