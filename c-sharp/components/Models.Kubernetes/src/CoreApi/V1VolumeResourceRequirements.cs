namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1VolumeResourceRequirements
    {
        public Dictionary<string, string>? Requests { get; set; }
        public Dictionary<string, string>? Limits { get; set; }
    }
}