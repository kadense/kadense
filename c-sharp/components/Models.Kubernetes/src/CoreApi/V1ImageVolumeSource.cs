namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ImageVolumeSource
    {
        public string? PullPolicy { get; set; }
        public string? Reference { get; set; }
    }
}