using System.Reflection;

namespace Kadense.Client.Kubernetes
{
    public class CustomResourceClientFactory
    {
        public KadenseCustomResourceClient<T> Create<T>(IKubernetes client)
            where T : KadenseCustomResource
        {
            var genericClient = CreateGenericClient<T>(client);
            return new KadenseCustomResourceClient<T>(genericClient);
        }

        
        public KadenseCustomResourceClient<T> Create<T>(GenericClient genericClient)
            where T : KadenseCustomResource
        {
            return new KadenseCustomResourceClient<T>(genericClient);
        }
        
        protected GenericClient CreateGenericClient<T>(IKubernetes client)
        {
            Type type = typeof(T);
            var attribute = (KubernetesCustomResourceAttribute)type.GetCustomAttributes(typeof(KubernetesCustomResourceAttribute), false).First();

            return new GenericClient(client, attribute.Group, attribute.Version, attribute.PluralName.ToLower());
        }
    }
}