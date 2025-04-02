using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;

namespace Kadense.Jupyternetes.Pods.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        public KadenseCustomResourceClient<JupyterNotebookTemplate> TemplateClient { get; set; }

        public WatcherService() : base()
        {
            var k8sClientFactory = new KubernetesClientFactory();
            var k8sClient = k8sClientFactory.CreateClient();
            var clientFactory = new CustomResourceClientFactory();
            this.TemplateClient = clientFactory.Create<JupyterNotebookTemplate>(k8sClient);
        }

        protected override async Task OnAddedAsync(JupyterNotebookInstance resource)
        {
            if (resource.Spec!.Template == null)
                return;

            var template = await this.GetTemplateAsync(resource);
        }

        protected override async Task OnUpdatedAsync(JupyterNotebookInstance resource)
        {
            if (resource.Spec!.Template == null)
                return;

            var template = await this.GetTemplateAsync(resource);
        }

        private async Task<JupyterNotebookTemplate?> GetTemplateAsync(JupyterNotebookInstance resource)
        {
            try {
                var templateName = resource.Spec!.Template!.Name!;
                return await this.TemplateClient.ReadNamespacedAsync(resource.Metadata.NamespaceProperty, templateName);
            }
            catch (k8s.Exceptions.KubernetesClientException ex)
            {
                // Handle the case where the template is not found
                return null;
            }
            catch(NullReferenceException ex){
                return null;
            }
        }

        protected override async Task OnDeletedAsync(JupyterNotebookInstance resource)
        {
            await Task.Run(() => { });
        }
    }
}