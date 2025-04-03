using k8s.Models;

namespace Kadense.Client.Kubernetes
{
    public abstract class KadenseCustomResourceWatcher<T>
        where T : KadenseCustomResource
    {
        protected KadenseCustomResourceClient<T> Client;
        protected IKubernetes K8sClient { get; set; }

        public KadenseCustomResourceWatcher()
        {
            var k8sClientFactory = new KubernetesClientFactory();
            this.K8sClient = k8sClientFactory.CreateClient();

            var clientFactory = new CustomResourceClientFactory();
            this.Client = clientFactory.Create<T>(this.K8sClient);
        }
        public KadenseCustomResourceWatcher(IKubernetes client)
        {
            this.K8sClient = client;
            var clientFactory = new CustomResourceClientFactory();
            this.Client = clientFactory.Create<T>(this.K8sClient);
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

        public abstract Task<(T?, k8s.Models.Corev1Event?)> OnAddedAsync(T item);
        public abstract Task<(T?, k8s.Models.Corev1Event?)> OnUpdatedAsync(T item);
        public abstract Task<(T?, k8s.Models.Corev1Event?)> OnDeletedAsync(T item);
        protected virtual void OnError()
        {
            // Default implementation for handling errors
        }

        protected virtual async Task<Corev1Event> CreateEventAsync(V1ObjectReference involvedObject, string action, string reason, Corev1EventSeries? series = null, V1ObjectReference? related = null, string type = "Normal", string? message = null)
        {
            var body = new k8s.Models.Corev1Event(
                    metadata: new V1ObjectMeta(
                        generateName: $"{involvedObject.Name}.",
                        namespaceProperty: involvedObject.NamespaceProperty
                    ),
                    reportingComponent: System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name,
                    reportingInstance: Environment.MachineName,
                    eventTime: DateTime.UtcNow,
                    action: action,
                    involvedObject: involvedObject,
                    related: related,
                    reason: reason,
                    message: message,
                    type: type
                );

            var evt = await this.K8sClient.CoreV1.CreateNamespacedEventAsync(
                namespaceParameter: involvedObject.NamespaceProperty,
                body: body
            );

            return evt;
        }
    }
}