namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ManagedFieldsEntry : KadenseTemplatedCopiedResource<k8s.Models.V1ManagedFieldsEntry>
    {
        [JsonPropertyName("manager")]
        public string? Manager { get; set; }

        [JsonPropertyName("operation")]
        public string? Operation { get; set; }

        [JsonPropertyName("apiVersion")]
        public string? ApiVersion { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("fieldsType")]
        public string? FieldsType { get; set; }

        [JsonPropertyName("fieldsV1")]
        public object? FieldsV1 { get; set; }

        public override k8s.Models.V1ManagedFieldsEntry ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ManagedFieldsEntry(
                manager: this.GetValue(this.Manager, variables),
                operation: this.GetValue(this.Operation, variables),
                apiVersion: this.GetValue(this.ApiVersion, variables),
                time: this.Time != null ? DateTime.Parse(this.GetValue(this.Time, variables)!) : null,
                fieldsType: this.GetValue(this.FieldsType, variables),
                fieldsV1: this.FieldsV1
            );
        }

    }
}