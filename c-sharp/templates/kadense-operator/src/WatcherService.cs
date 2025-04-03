using Kadense.Client.Kubernetes;

using Kadense.Models.Kubernetes;

namespace Kadense.Template.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<ExampleResource>
    {
        public override async Task<(ExampleResource?, k8s.Models.Corev1Event?)> OnAddedAsync(ExampleResource resource)
        {
            // Handle resource added logic here
            await Task.Run(() => { });

            return (null, null);
        }

        public override async Task<(ExampleResource?, k8s.Models.Corev1Event?)> OnUpdatedAsync(ExampleResource resource)
        {
            // Handle resource updated logic here
            await Task.Run(() => { });
            return (null, null);
        }

        public override async Task<(ExampleResource?, k8s.Models.Corev1Event?)> OnDeletedAsync(ExampleResource resource)
        {
            // Handle resource deleted logic here
            await Task.Run(() => { });
            return (null, null);
        }
    }
}