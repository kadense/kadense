namespace Kadense.Client.Kubernetes
{
    public abstract class KadenseCustomResourceWatcher<T>
        where T : KadenseCustomResource
    {
        protected KadenseCustomResourceClient<T> Client;

        public KadenseCustomResourceWatcher()
        {
            this.Client = this.InitialiseClient();
        }
        public KadenseCustomResourceWatcher(IKubernetes client)
        {
            this.Client = this.InitialiseClient(client);
        }

        public KadenseCustomResourceWatcher(KadenseCustomResourceClient<T> client)
        {
            this.Client = client;
        }

        protected virtual KadenseCustomResourceClient<T> InitialiseClient()
        {
            var clientFactory = new KubernetesClientFactory();
            return this.InitialiseClient(clientFactory.CreateClient()); 
        }

        protected virtual KadenseCustomResourceClient<T> InitialiseClient(IKubernetes k8sClient)
        {
            var clientFactory = new CustomResourceClientFactory();
            return clientFactory.Create<T>(k8sClient);
        }


        public void Start()
        {
            this.Client.Watch(onEvent: OnWatchEvent, onError: OnWatchError, onClosed: OnWatchStopped);
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
                    OnAddedAsync(item).Start();
                    break;

                case WatchEventType.Modified:
                    OnUpdatedAsync(item).Start();
                    break;

                case WatchEventType.Deleted:
                    OnDeletedAsync(item).Start();
                    break;

                case WatchEventType.Error:
                    OnError();
                    break;

                default:
                    throw new NotImplementedException($"WatchEventType {watchEventType} is not recognised");
            }
        }

        protected abstract Task OnAddedAsync(T item);
        protected abstract Task OnUpdatedAsync(T item);
        protected abstract Task OnDeletedAsync(T item);
        protected virtual void OnError()
        {
            // Default implementation for handling errors
        }
    }
}