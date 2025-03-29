namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapProjection
    {
        public string? Name { get; set; }
        public bool? Optional { get; set; }
        public List<V1KeyToPath>? Items { get; set; }
    }
}