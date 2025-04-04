using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;
using k8s;
using Kadense.Logging;
using k8s.Models;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Kadense.Jupyternetes.Pods.Operator 
{
    public class PodWatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        private readonly KadenseLogger<PodWatcherService> _logger;

        public KadenseCustomResourceClient<JupyterNotebookTemplate> TemplateClient { get; set; }

        public Kadense.Models.Kubernetes.KubernetesCustomResourceAttribute CustomResourceAttribute { get; set; }

        public PodWatcherService(IKadenseLogger logger) : base()
        {
            var type = typeof(JupyterNotebookInstance);
            this.CustomResourceAttribute = type.GetCustomAttributes<Kadense.Models.Kubernetes.KubernetesCustomResourceAttribute>(true).First()!;
            _logger = logger.Create<PodWatcherService>();
            var k8sClientFactory = new KubernetesClientFactory();
            this.K8sClient = k8sClientFactory.CreateClient();
            var clientFactory = new CustomResourceClientFactory();
            this.TemplateClient = clientFactory.Create<JupyterNotebookTemplate>(this.K8sClient);
        }

        public override async Task<(JupyterNotebookInstance?, k8s.Models.Corev1Event?)> OnAddedAsync(JupyterNotebookInstance resource)
        {
            _logger.LogInformation("Resource created: {ResourceName}", resource.Metadata.Name);

            await this.ProcessAsync(resource);

            var evt = await this.CreateEventAsync(
                involvedObject: resource.ToV1ObjectReference(),
                action: "Processed Resource Created",
                reason: $"Resource Created"
            );

            return (resource, evt);
        }

        public override async Task<(JupyterNotebookInstance?, k8s.Models.Corev1Event?)> OnUpdatedAsync(JupyterNotebookInstance resource)
        {
            _logger.LogInformation("Resource updated: {ResourceName}", resource.Metadata.Name);

            var evt = await this.CreateEventAsync(
                involvedObject: resource.ToV1ObjectReference(),
                action: "Processed Resource Updated",
                reason: "Resource Updated"
            );
            return (resource, evt);
        }

        public async Task ProcessAsync(JupyterNotebookInstance resource)
        {
            if (resource.Spec!.Template == null)
            {
                _logger.LogWarning("Resource {ResourceName} has no template specified.", resource.Metadata.Name);
                return;
            }

            var template = await this.GetTemplateAsync(resource);

            if (template == null)
            {
                _logger.LogError("Template not found for resource {ResourceName}.", resource.Metadata.Name);
                return;
            }


            _logger.LogInformation("Building pods for resource {ResourceName}.", resource.Metadata.Name);
            bool updated = await BuildPodsAsync(resource, template);

            if(!resource.Status.PodsProvisioningState.Equals("Completed"))
            {
                resource.Status.PodsProvisioningState = "Completed";
                updated = true;
            }

            if (updated)
            {
                _logger.LogInformation("Updating resource {ResourceName} status.", resource.Metadata.Name);
                await UpdateStatusAsync(resource);
            }
            else
            {
                _logger.LogInformation("No updates needed for resource {ResourceName}.", resource.Metadata.Name);
            }
        }

        private async Task UpdateStatusAsync(JupyterNotebookInstance resource){
            await this.K8sClient.CustomObjects.PatchNamespacedCustomObjectStatusAsync(
                body: new k8s.Models.V1Patch(
                    new Dictionary<string, object>()
                    {
                        { "status", resource.Status }
                    },
                    type: V1Patch.PatchType.MergePatch
                ),
                group: this.CustomResourceAttribute.Group,
                version: this.CustomResourceAttribute.Version,
                plural: this.CustomResourceAttribute.PluralName.ToLower(),
                namespaceParameter: resource.Metadata.NamespaceProperty!,
                name: resource.Metadata.Name!
            );
            _logger.LogInformation("Patched Status {ResourceName}.", resource.Metadata.Name);

        }

        private async Task<bool> BuildPodsAsync(JupyterNotebookInstance resource, JupyterNotebookTemplate template)
        {
            bool updated = false;
            List<string> podNames = new List<string>();

            _logger.LogInformation("Creating pods for resource {ResourceName} using template {TemplateName}.", resource.Metadata.Name, template.Metadata.Name);

            var pods = template.CreatePods(resource);
            foreach (var pod in pods)
            {
                string podName = pod.Metadata.Labels["jupyternetes.kadense.io/podName"];
                var podInK8s = await CreatePodIfNotExists(resource, pod, resource.Metadata.NamespaceProperty!);
                podNames.Add(podName);
                if(resource.Status!.Pods!.ContainsKey(podName))
                {
                    if (resource.Status.Pods[podName] != podInK8s!.Metadata.Name)
                    {
                        resource.Status.Pods[podName] = podInK8s!.Metadata.Name;
                        updated = true;
                    }
                }
                else
                {
                    resource.Status.Pods.Add(podName, podInK8s!.Metadata.Name);
                    updated = true;
                }   
            }

            resource.Status!.Pods!.Where(x => !podNames.Contains(x.Key)).ToList().ForEach(x => {
                resource.Status.Pods!.Remove(x.Key);
                updated = true;
            });

            return updated;
        }

        private async Task<k8s.Models.V1Pod?> CreatePodIfNotExists(JupyterNotebookInstance resource, k8s.Models.V1Pod pod, string namespaceName)
        {
            _logger.LogInformation("Checking if pod {PodName} exists in namespace {Namespace}.", pod.Metadata.GenerateName ?? pod.Metadata.Name, namespaceName);

            var labelSelector = $"jupyternetes.kadense.io/template={pod.Metadata.Labels["jupyternetes.kadense.io/template"]},jupyternetes.kadense.io/templateNamespace={pod.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"]},jupyternetes.kadense.io/instance={pod.Metadata.Labels["jupyternetes.kadense.io/instance"]},jupyternetes.kadense.io/instanceNamespace={pod.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"]}";
            var existingPods = await this.K8sClient.CoreV1.ListNamespacedPodAsync(namespaceName, labelSelector: labelSelector);
            var filteredPods = existingPods.Items.Where(x => x.Metadata.Name == pod.Metadata.Name);
            if (filteredPods.Count() > 0)
            {
                _logger.LogInformation("Pod {PodName} already exists in namespace {Namespace}.", pod.Metadata.GenerateName ?? pod.Metadata.Name, namespaceName);
                return filteredPods.First();
            }

            _logger.LogInformation("Creating pod {PodName} in namespace {Namespace}.", pod.Metadata.GenerateName ?? pod.Metadata.Name, namespaceName);
            pod = await this.K8sClient.CoreV1.CreateNamespacedPodAsync(pod, namespaceName);
            
            await this.CreateEventAsync(
                action: "Pod Created",
                related: resource.ToV1ObjectReference(),
                reason: "Pod Creation Triggered",
                involvedObject: new k8s.Models.V1ObjectReference(
                    kind: "Pod",
                    name: pod.Metadata.Name,
                    namespaceProperty: namespaceName
                ),
                message: $"Created from JupyterNotebookInstance/{resource.Metadata.Name}"
            );

            return pod;
        }

        private async Task<JupyterNotebookTemplate?> GetTemplateAsync(JupyterNotebookInstance resource)
        {
            try 
            {
                var templateName = resource.Spec!.Template!.Name!;
                _logger.LogInformation("Fetching template {TemplateName} for resource {ResourceName}.", templateName, resource.Metadata.Name);
                return await this.TemplateClient.ReadNamespacedAsync(resource.Metadata.NamespaceProperty, templateName);
            }
            catch (k8s.Exceptions.KubernetesClientException ex)
            {
                _logger.LogError(ex, "Error fetching template for resource {ResourceName}.", resource.Metadata.Name);
                return null;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference encountered while fetching template for resource {ResourceName}.", resource.Metadata.Name);
                return null;
            }
        }

        public override async Task<(JupyterNotebookInstance?, k8s.Models.Corev1Event?)> OnDeletedAsync(JupyterNotebookInstance resource)
        {
            _logger.LogInformation("Resource deleted: {ResourceName}", resource.Metadata.Name);

            var existingPods = await this.K8sClient.CoreV1.ListNamespacedPodAsync(resource.Metadata.NamespaceProperty, labelSelector: $"jupyternetes.kadense.io/instance={resource.Metadata.Name},jupyternetes.kadense.io/instanceNamespace={resource.Metadata.NamespaceProperty}");
            foreach(var pod in existingPods.Items)
            {
                _logger.LogInformation("Deleting pod {PodName} in namespace {Namespace}.", pod.Metadata.Name, resource.Metadata.NamespaceProperty);
                await this.K8sClient.CoreV1.DeleteNamespacedPodAsync(
                    name: pod.Metadata.Name!, 
                    namespaceParameter: resource.Metadata.NamespaceProperty
                );

                await this.CreateEventAsync(
                    related: resource.ToV1ObjectReference(),
                    action: "Deleted by Jupyternetes Pod Operator",
                    reason: "Pod Deleted",
                    involvedObject: new k8s.Models.V1ObjectReference(
                        kind: "Pod",
                        name: pod.Metadata.Name,
                        namespaceProperty: resource.Metadata.NamespaceProperty
                    ),
                    message: $"Deleted by JupyterNotebookInstance/{resource.Metadata.Name} Resource Deletion"
                );
            }

            var evt = await this.CreateEventAsync(
                involvedObject: resource.ToV1ObjectReference(),
                action: "Processed Resource Deletion",
                reason: "Resource Deleted"
            );

            return (resource, evt);
        }
    }
}