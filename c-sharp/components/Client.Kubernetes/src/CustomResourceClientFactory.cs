using System.Reflection;

namespace Kadense.Client.Kubernetes
{
    public class CustomResourceClientFactory
    {
        public GenericClient Create<T>(IKubernetes client)
        {
            Type type = typeof(T);
            var attribute = (KubernetesCustomResourceAttribute)type.GetCustomAttributes(typeof(KubernetesCustomResourceAttribute), false).First();

            return new GenericClient(client, attribute.Group, attribute.Version, attribute.PluralName.ToLower());
        }
    }
}