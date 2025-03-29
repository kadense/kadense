namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Toleration
    {
        public string? Key { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public string? Effect { get; set; }
        public int? TolerationSeconds { get; set; }
    }
}