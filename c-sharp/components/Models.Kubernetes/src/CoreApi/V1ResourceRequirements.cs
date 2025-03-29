namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ResourceRequirements
    {
        public List<V1ResourceClaim>? Claims { get; set; }
        public Dictionary<string, string>? Limits { get; set; }
        public Dictionary<string, string>? Requests { get; set; }
    }
}