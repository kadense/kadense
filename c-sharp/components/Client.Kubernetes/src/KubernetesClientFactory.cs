using k8s;
using K8sClient = k8s.Kubernetes;

namespace Kadense.Client.Kubernetes
{
    
    /// <summary>
    /// Factory class for creating instances of <see cref="IKubernetes"/>.
    /// </summary>
    /// <remarks>
    /// This class provides a method to create a Kubernetes client instance based on the environment
    /// in which the application is running. If the application is running inside a Kubernetes cluster,
    /// it uses the in-cluster configuration. Otherwise, it uses a configuration file to create the client.
    /// </remarks>
    public class KubernetesClientFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IKubernetes"/> based on the current environment.
        /// </summary>
        /// <returns>An <see cref="IKubernetes"/> client instance.</returns>
        public virtual IKubernetes CreateClient()
        {
            if (IsRunningInKubernetes())
            {
                var config = KubernetesClientConfiguration.InClusterConfig();
                return new K8sClient(config);
            }
            else
            {
                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
                return new K8sClient(config);
            }
        }

        /// <summary>
        /// Determines if the application is running inside a Kubernetes cluster.
        /// </summary>
        /// <returns><c>true</c> if running inside a Kubernetes cluster; otherwise, <c>false</c>.</returns>
        public bool IsRunningInKubernetes()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST"));
        }
    }
}