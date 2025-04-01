namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1HTTPGetAction : KadenseTemplatedCopiedResource<k8s.Models.V1HTTPGetAction>
    {
        public string? Path { get; set; }
        public string? Host { get; set; }
        public int? Port { get; set; }
        public string? Scheme { get; set; }
        public List<V1HTTPHeader>? HttpHeaders { get; set; }

        public override k8s.Models.V1HTTPGetAction ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1HTTPGetAction(
                path: this.GetValue(this.Path, variables),
                port: this.Port,
                host: this.GetValue(this.Host, variables),
                scheme: this.GetValue(this.Scheme, variables),
                httpHeaders: this.GetValue<V1HTTPHeader, k8s.Models.V1HTTPHeader>(this.HttpHeaders, variables)
            );
        }
    }
}