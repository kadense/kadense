namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ServiceAccountTokenProjection
    {
        public string? Path { get; set; }
        public int? ExpirationSeconds { get; set; }
        public string? Audience { get; set; }
    }
}