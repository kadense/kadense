namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretProjection
    {
        public List<V1KeyToPath>? Items { get; set; }
        public string? Name { get; set; }
        public bool? Optional { get; set; }
    }
}