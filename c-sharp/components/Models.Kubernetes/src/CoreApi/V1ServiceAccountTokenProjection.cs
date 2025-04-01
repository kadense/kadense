
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ServiceAccountTokenProjection : KadenseTemplatedCopiedResource<k8s.Models.V1ServiceAccountTokenProjection>
    {
        public string? Path { get; set; }
        public long? ExpirationSeconds { get; set; }
        public string? Audience { get; set; }

        public override k8s.Models.V1ServiceAccountTokenProjection ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ServiceAccountTokenProjection(
                path: this.GetValue(this.Path, variables),
                expirationSeconds: this.ExpirationSeconds,
                audience: this.GetValue(this.Audience, variables)
            );
        }
    }
}