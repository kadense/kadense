namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SELinuxOptions
    {
        public string? User { get; set; }
        public string? Role { get; set; }
        public string? Type { get; set; }
        public string? Level { get; set; }
    }
}