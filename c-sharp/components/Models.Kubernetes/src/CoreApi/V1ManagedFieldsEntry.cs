namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ManagedFieldsEntry
    {
        public string? Manager { get; set; }
        public string? Operation { get; set; }
        public string? ApiVersion { get; set; }
        public string? Time { get; set; }
        public string? FieldsType { get; set; }
        public object? FieldsV1 { get; set; }

    }
}