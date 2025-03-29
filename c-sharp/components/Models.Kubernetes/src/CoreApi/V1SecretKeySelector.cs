namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretKeySelector
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
        public bool? Optional { get; set; }
    }
}