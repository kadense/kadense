using System.Security.Cryptography.X509Certificates;
using Kadense.Logging;
using Kadense.Jupyternetes.Watchers;

namespace Kadense.Jupyternetes.Pvcs.Operator
{
    public class Operator
    {
        public static void Main(string[] args)
        {
            KadenseLogger<Operator> logger = new KadenseLogger<Operator>();
            logger.LogInformation($"Starting Jupyternetes Pvcs Operator Version {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}");
            var pvcWatcherService = new JupyternetesPvcWatcherService(logger);
            pvcWatcherService.StartAndWait();
        }
    }
}