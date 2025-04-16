using System.Reflection;

namespace Kadense.Client.Kubernetes
{
    public class KadenseCustomResourceClient<T>
        where T : KadenseCustomResource
    {
        public GenericClient Client { get; }

        public KadenseCustomResourceClient(GenericClient client)
        {
            this.Client = client;
        }

        public async Task<T> ReadAsync(string name)
        {
            return await Client.ReadAsync<T>(name);
        }

        public async Task<T> ReadNamespacedAsync(string @namespace, string name)
        {
            return await Client.ReadNamespacedAsync<T>(@namespace, name);
        }

        public async Task<KadenseCustomResourceList<T>> ListAsync()
        {
            return await Client.ListAsync<KadenseCustomResourceList<T>>();
        }
 
        public async Task<KadenseCustomResourceList<T>> ListNamespacedAsync(string @namespace)
        {
            return await Client.ListNamespacedAsync<KadenseCustomResourceList<T>>(@namespace);
        }
        
        public async Task<T> CreateAsync(T resource)
        {
            return await Client.CreateAsync<T>(resource);
        }
        
        public async Task<T> CreateNamespacedAsync(T resource)
        {
            return await Client.CreateNamespacedAsync<T>(resource, resource.Metadata.NamespaceProperty);
        }

        public async Task<T> ReplaceAsync(T resource)
        {
            return await Client.ReplaceAsync<T>(resource, resource.Metadata.Name);
        }

        public async Task<T> ReplaceNamespacedAsync(T resource)
        {
            return await Client.ReplaceNamespacedAsync<T>(resource, resource.Metadata.NamespaceProperty, resource.Metadata.Name);
        }

        public async Task<T> DeleteAsync(string name)
        {
            return await Client.DeleteAsync<T>(name);
        }

        public async Task<T> DeleteNamespacedAsync(string @namespace, string name)
        {
            return await Client.DeleteNamespacedAsync<T>(@namespace, name);
        }

        public async Task<T> PatchAsync(k8s.Models.V1Patch patch, string name)
        {
            return await Client.PatchAsync<T>(patch, name);
        }

        public async Task<T> PatchNamespacedAsync(k8s.Models.V1Patch patch, string @namespace, string name)
        {
            return await Client.PatchNamespacedAsync<T>(patch, @namespace, name);
        }

        public Watcher<T> Watch(Action<WatchEventType, T> onEvent, Action<Exception> onError, Action onClosed)
        {
            return Client.Watch<T>(onEvent, onError, onClosed);
        }
    }
}