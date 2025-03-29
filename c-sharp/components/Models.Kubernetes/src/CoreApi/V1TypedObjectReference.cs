namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1TypedObjectReference
    {
        public string? ApiGroup { get; set; }
        public string? Kind { get; set; }
        public string? Name { get; set; }
        public string? Namespace { get; set; }
    }
}