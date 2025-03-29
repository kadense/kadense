namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HostAlias
    {
        public string? Ip { get; set; }
        public List<string>? Hostnames { get; set; }
    }
}