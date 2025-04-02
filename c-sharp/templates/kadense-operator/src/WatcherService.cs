using Kadense.Client.Kubernetes;

using Kadense.Models.Kubernetes;

namespace Kadense.Template.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<ExampleResource>
    {
        protected override async Task OnAddedAsync(ExampleResource resource)
        {
            // Handle resource added logic here
            await Task.Run(() => { });
        }

        protected override async Task OnUpdatedAsync(ExampleResource resource)
        {
            // Handle resource updated logic here
            await Task.Run(() => { });
        }

        protected override async Task OnDeletedAsync(ExampleResource resource)
        {
            // Handle resource deleted logic here
            await Task.Run(() => { });
        }
    }
}