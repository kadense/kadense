namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Lifecycle
    {
        public V1LifecycleHandler? PostStart { get; set; }
        public V1LifecycleHandler? PreStop { get; set; }
    }
}