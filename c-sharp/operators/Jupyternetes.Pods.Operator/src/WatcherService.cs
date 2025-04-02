using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;
using k8s;

namespace Kadense.Jupyternetes.Pods.Operator 
{
    public class WatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        public KadenseCustomResourceClient<JupyterNotebookTemplate> TemplateClient { get; set; }
        public IKubernetes K8sClient { get; set; }

        public WatcherService() : base()
        {
            var k8sClientFactory = new KubernetesClientFactory();
            this.K8sClient = k8sClientFactory.CreateClient();
            var clientFactory = new CustomResourceClientFactory();
            this.TemplateClient = clientFactory.Create<JupyterNotebookTemplate>(this.K8sClient);
        }

        protected override async Task OnAddedAsync(JupyterNotebookInstance resource)
        {
            await this.OnUpdatedAsync(resource);
        }

        protected override async Task OnUpdatedAsync(JupyterNotebookInstance resource)
        {
            if (resource.Spec!.Template == null)
                return;

            var template = await this.GetTemplateAsync(resource);

            if (template == null)
            {
                return;
            }
        }

        private async Task BuildPodsAsync(JupyterNotebookInstance resource, JupyterNotebookTemplate template)
        {

            var pods = template.CreatePods(resource);

            foreach (var pod in pods)
            {
                await CreatePodIfNotExists(pod, resource.Metadata.NamespaceProperty!);
            }
        }

        private async Task CreatePodIfNotExists(k8s.Models.V1Pod pod, string namespaceName)
        {
            var existingPods = await this.K8sClient.CoreV1.ListNamespacedPodAsync(namespaceName, labelSelector: $"jupyternetes.kadense.io/template={pod.Metadata.Labels["jupyternetes.kadense.io/template"]}&jupyternetes.kadense.io/templateNamespace={pod.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"]}&jupyternetes.kadense.io/instance={pod.Metadata.Labels["jupyternetes.kadense.io/instance"]}&jupyternetes.kadense.io/instanceNamespace={pod.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"]}");
            var filteredPods = existingPods.Items.Where(x => x.Metadata.Name == pod.Metadata.Name);
            if (filteredPods.Count() == 0)
                return;

            await this.K8sClient.CoreV1.CreateNamespacedPodAsync(pod, namespaceName);
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