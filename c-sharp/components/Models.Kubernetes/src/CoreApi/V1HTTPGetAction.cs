namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HTTPGetAction
    {
        public string? Path { get; set; }
        public string? Host { get; set; }
        public int? Port { get; set; }
        public string? Scheme { get; set; }
        public List<V1HTTPHeader>? HttpHeaders { get; set; }
    }
}