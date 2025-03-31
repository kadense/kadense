namespace Kadense.Client.Kubernetes
{
    public abstract class KadenseCustomResourceWatcher<T>
        where T : KadenseCustomResource
    {
        protected GenericClient Client;

        public KadenseCustomResourceWatcher()
        {
            this.Client = this.InitialiseClient();
        }
        public KadenseCustomResourceWatcher(IKubernetes client)
        {
            this.Client = this.InitialiseClient(client);
        }

        public KadenseCustomResourceWatcher(GenericClient client)
        {
            this.Client = client;
        }

        protected virtual GenericClient InitialiseClient()
        {
            var clientFactory = new KubernetesClientFactory();
            return this.InitialiseClient(clientFactory.CreateClient()); 
        }

        protected virtual GenericClient InitialiseClient(IKubernetes k8sClient)
        {
            var clientFactory = new CustomResourceClientFactory();
            return clientFactory.Create<T>(k8sClient);
        }


        public void Start()
        {
            this.Client.Watch<T>(onEvent: OnWatchEvent, onError: OnWatchError, onClosed: OnWatchStopped);
        }

        protected void OnWatchError(Exception ex)
        {
            throw ex;
        }

        protected void OnWatchStopped()
        {

        }

        protected virtual void OnWatchEvent(WatchEventType watchEventType, T item)
        {
            switch (watchEventType)
            {
                case WatchEventType.Added:
                    OnAdded(item);
                    break;

                case WatchEventType.Modified:
                    OnUpdated(item);
                    break;

                case WatchEventType.Deleted:
                    OnDeleted(item);
                    break;

                case WatchEventType.Error:
                    OnError();
                    break;

                default:
                    throw new NotImplementedException($"WatchEventType {watchEventType} is not recognised");
            }
        }

        protected abstract void OnAdded(T item);
        protected abstract void OnUpdated(T item);
        protected abstract void OnDeleted(T item);
        protected virtual void OnError()
        {
            // Default implementation for handling errors
        }
    }
}