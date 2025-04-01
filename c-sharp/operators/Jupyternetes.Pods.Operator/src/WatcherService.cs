using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;

namespace Kadense.Jupyternetes.Pods.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        public WatcherService() : base()
        {
            
        }
        
        protected override void OnAdded(JupyterNotebookInstance resource)
        {
            var template = resource.Spec!.Template.Name;
            
        }

        protected override void OnUpdated(JupyterNotebookInstance resource)
        {
        }

        protected override void OnDeleted(JupyterNotebookInstance resource)
        {
            
        }
    }
}