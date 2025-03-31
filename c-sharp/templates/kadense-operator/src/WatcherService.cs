using Kadense.Client.Kubernetes;

using Kadense.Models.Kubernetes;

namespace Kadense.Template.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<ExampleResource>
    {
        protected override void OnAdded(ExampleResource resource)
        {
            // Handle resource added logic here
        }

        protected override void OnUpdated(ExampleResource resource)
        {
            // Handle resource updated logic here
        }

        protected override void OnDeleted(ExampleResource resource)
        {
            // Handle resource deleted logic here
        }
    }
}