using System.Security.Cryptography.X509Certificates;
using Kadense.Logging;

namespace Kadense.Jupyternetes.Pods.Operator
{
    public class Operator
    {
        public static void Main(string[] args)
        {
            KadenseLogger<Operator> logger = new KadenseLogger<Operator>();
            logger.LogInformation("Starting Jupyternetes Pods Operator...");
            var podWatcherService = new PodWatcherService(logger);

            var podWatcherTask = Task.Run(() => podWatcherService.Start());
            podWatcherService.Start();

            podWatcherTask.Wait();
        }
    }
}