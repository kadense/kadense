namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1AzureFileVolumeSource
    { 
        public string? SecretName { get; set; }
        public string? ShareName { get; set; }
        public bool? ReadOnly { get; set; }
    }
}