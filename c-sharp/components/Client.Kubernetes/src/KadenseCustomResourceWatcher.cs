using Kadense.Logging;

using k8s.Models;

namespace Kadense.Client.Kubernetes
{
    public abstract class KadenseCustomResourceWatcher<T> : KadenseResourceWatcher<T>
        where T : KadenseCustomResource, IKubernetesObject<V1ObjectMeta>
    {
        private readonly KadenseLogger<KadenseCustomResourceWatcher<T>> _logger;
        protected KadenseCustomResourceClient<T> Client;

        public KadenseCustomResourceWatcher()
        {
            _logger = new KadenseLogger<KadenseCustomResourceWatcher<T>>();
            var k8sClientFactory = new KubernetesClientFactory();
            this.K8sClient = k8sClientFactory.CreateClient();
            var clientFactory = new CustomResourceClientFactory();
            this.Client = clientFactory.Create<T>(this.K8sClient);
            this.GenericClient = this.Client.Client;            
        } 
        public KadenseCustomResourceWatcher(IKubernetes client) : base(client)
        {
            _logger = new KadenseLogger<KadenseCustomResourceWatcher<T>>();
            var clientFactory = new CustomResourceClientFactory();
            this.K8sClient = client;
            this.Client = clientFactory.Create<T>(this.K8sClient);
            this.GenericClient = this.Client.Client;            
        }
    }
}