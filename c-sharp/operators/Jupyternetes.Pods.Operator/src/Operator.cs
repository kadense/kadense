using System.Security.Cryptography.X509Certificates;
using Kadense.Logging;
using Kadense.Jupyternetes.Watchers;

namespace Kadense.Jupyternetes.Pods.Operator
{
    public class Operator
    {
        public static void Main(string[] args)
        {
            KadenseLogger<Operator> logger = new KadenseLogger<Operator>();
            logger.LogInformation($"Starting Jupyternetes Pods Operator Version {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}");
            var k8sPodsWatcher = new K8sPodsWatcher();
            Task task = k8sPodsWatcher.Start();
            var podWatcherService = new JupyternetesPodWatcherService(logger);
            podWatcherService.StartAndWait();
            task.Wait();
        }
    }
}