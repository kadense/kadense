namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Lifecycle : KadenseTemplatedCopiedResource<k8s.Models.V1Lifecycle>
    {
        [JsonPropertyName("postStart")]
        public V1LifecycleHandler? PostStart { get; set; }

        [JsonPropertyName("preStop")]
        public V1LifecycleHandler? PreStop { get; set; }

        public override k8s.Models.V1Lifecycle ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Lifecycle(
                postStart: this.PostStart != null ? this.PostStart.ToOriginal(variables) : null,
                preStop: this.PreStop != null ? this.PreStop.ToOriginal(variables) : null
            );
        }
    }
}