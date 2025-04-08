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
            var podWatcherService = new PodWatcherService(logger);
            podWatcherService.StartAndWait();
        }
    }
}