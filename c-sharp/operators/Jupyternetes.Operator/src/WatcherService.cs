using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;

namespace Kadense.Jupyternetes.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        protected override void OnAdded(JupyterNotebookInstance resource)
        {
            // Handle resource added logic here
        }

        protected override void OnUpdated(JupyterNotebookInstance resource)
        {
            // Handle resource updated logic here
        }

        protected override void OnDeleted(JupyterNotebookInstance resource)
        {
            // Handle resource deleted logic here
        }
    }
}