using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes;
using Kadense.Models.Jupyternetes;
using k8s;
using Kadense.Logging;
using k8s.Models;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Kadense.Jupyternetes.Watchers
{
    public class JupyternetesPvcWatcherService : KadenseCustomResourceWatcher<JupyterNotebookInstance>
    {
        private readonly string STATE_COMPLETED = "Completed";
        private readonly string STATE_PARTIALLY_PROCESSED = "Partially Processed";
        private readonly KadenseLogger<JupyternetesPvcWatcherService> _logger;

        public KadenseCustomResourceClient<JupyterNotebookTemplate> TemplateClient { get; set; }

        public Kadense.Models.Kubernetes.KubernetesCustomResourceAttribute CustomResourceAttribute { get; set; }

        public JupyternetesPvcWatcherService(IKadenseLogger logger) : base()
        {
            var type = typeof(JupyterNotebookInstance);
            this.CustomResourceAttribute = type.GetCustomAttributes<Kadense.Models.Kubernetes.KubernetesCustomResourceAttribute>(true).First()!;
            _logger = logger.Create<JupyternetesPvcWatcherService>();
            var k8sClientFactory = new KubernetesClientFactory();
            this.K8sClient = k8sClientFactory.CreateClient();
            var clientFactory = new CustomResourceClientFactory();
            this.TemplateClient = clientFactory.Create<JupyterNotebookTemplate>(this.K8sClient);
        }
        public JupyternetesPvcWatcherService(IKubernetes k8sClient, IKadenseLogger logger) : base(k8sClient)
        {
            var type = typeof(JupyterNotebookInstance);
            this.CustomResourceAttribute = type.GetCustomAttributes<Kadense.Models.Kubernetes.KubernetesCustomResourceAttribute>(true).First()!;
            _logger = logger.Create<JupyternetesPvcWatcherService>();
            this.K8sClient = k8sClient;
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

            await this.ProcessAsync(resource);

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


            _logger.LogInformation("Building pvcs for resource {ResourceName}.", resource.Metadata.Name);
            var (updated, provisioningState) = await BuildPvcsAsync(resource, template);

            if(!resource.Status.PvcsProvisionedState.Equals(provisioningState))
            {
                resource.Status.PvcsProvisionedState = provisioningState;
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

        private bool UpdateResourceState(JupyterNotebookInstance resource, string name, string? resourceName = null, string? state = null, string? errorMessage = null)
        {
            bool updated = false;
            bool statusExists = false;
            resource.Status!.Pvcs!.Where(x => x.Key!.Equals(name)).ToList().ForEach(x => {
                if (x.Value.ResourceName != resourceName)
                {
                    x.Value.ResourceName = resourceName;
                    x.Value.State = state;
                    x.Value.ErrorMessage = errorMessage;
                    updated = true;
                }
                statusExists = true;
            });
            if(!statusExists)
            {
                resource.Status.Pvcs.Add(name, new JupyterResourceState(resourceName: null, state: state, errorMessage: errorMessage));
                updated = true;
            }
            return updated;
        }

        private async Task<(bool, string)> BuildPvcsAsync(JupyterNotebookInstance resource, JupyterNotebookTemplate template)
        {
            bool updated = false;
            List<string> pvcNames = new List<string>();

            _logger.LogInformation("Creating pvcs for resource {ResourceName} using template {TemplateName}.", resource.Metadata.Name, template.Metadata.Name);

            var (pvcs, conversionIssues) = template.CreatePvcs(resource);
            foreach (var pvc in pvcs)
            {
                string pvcName = pvc.Metadata.Labels["jupyternetes.kadense.io/pvcName"];
                var pvcInK8s = await CreatePvcIfNotExists(resource, pvc, resource.Metadata.NamespaceProperty!);
                pvcNames.Add(pvcName);
                if(UpdateResourceState(resource, pvcName, pvcInK8s?.Metadata.Name, "Processed"))
                {
                    updated = true;
                }    
            }

            resource.Status!.Pvcs!.Where(x => !pvcNames.Contains(x.Key)).ToList().ForEach(x => {
                resource.Status.Pvcs!.Remove(x.Key);
                updated = true;
            });

            foreach(var issue in conversionIssues)
            {
                if(UpdateResourceState(resource, issue.Key, null, "Error", errorMessage: issue.Value.Message))
                {
                    updated = true;
                }
            }

            string provisioningState = conversionIssues.Count > 0 ? STATE_PARTIALLY_PROCESSED : STATE_COMPLETED;

            return (updated, provisioningState);
        }

        private async Task<k8s.Models.V1PersistentVolumeClaim?> CreatePvcIfNotExists(JupyterNotebookInstance resource, k8s.Models.V1PersistentVolumeClaim pvc, string namespaceName)
        {
            _logger.LogInformation("Checking if Pvc {PvcName} exists in namespace {Namespace}.", pvc.Metadata.GenerateName ?? pvc.Metadata.Name, namespaceName);

            var labelSelector = new KubernetesLabelSelector()
            {
                { "jupyternetes.kadense.io/template", pvc.Metadata.Labels["jupyternetes.kadense.io/template"] },
                { "jupyternetes.kadense.io/templateNamespace", pvc.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"] },
                { "jupyternetes.kadense.io/instance", pvc.Metadata.Labels["jupyternetes.kadense.io/instance"] },
                { "jupyternetes.kadense.io/instanceNamespace", pvc.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"] },
                { "jupyternetes.kadense.io/pvcName", pvc.Metadata.Labels["jupyternetes.kadense.io/pvcName"] }
            };
            var existingPvcs = await this.K8sClient.CoreV1.ListNamespacedPersistentVolumeClaimAsync(namespaceName, labelSelector: labelSelector.ToString());
            if (existingPvcs.Items.Count > 0)
            {
                _logger.LogInformation("Pvc {PvcName} already exists in namespace {Namespace}.", pvc.Metadata.GenerateName ?? pvc.Metadata.Name, namespaceName);
                return existingPvcs.Items.First();
            }

            _logger.LogInformation("Creating Pvc {PvcName} in namespace {Namespace}.", pvc.Metadata.GenerateName ?? pvc.Metadata.Name, namespaceName);
            pvc = await this.K8sClient.CoreV1.CreateNamespacedPersistentVolumeClaimAsync(pvc, namespaceName);
            
            await this.CreateEventAsync(
                action: "Pvc Created",
                related: resource.ToV1ObjectReference(),
                reason: "Pvc Creation Triggered",
                involvedObject: new k8s.Models.V1ObjectReference(
                    kind: "Pvc",
                    name: pvc.Metadata.Name,
                    namespaceProperty: namespaceName
                ),
                message: $"Created from JupyterNotebookInstance/{resource.Metadata.Name}"
            );

            return pvc;
        }

        private async Task<JupyterNotebookTemplate?> GetTemplateAsync(JupyterNotebookInstance resource)
        {
            try 
            {
                var templateName = resource.Spec!.Template!.Name!;
                var templateNamespace = resource.Spec.Template.Namespace ?? resource.Metadata.NamespaceProperty!;
                _logger.LogInformation("Fetching template {TemplateName} for resource {ResourceName}.", templateName, templateNamespace);
                return await this.TemplateClient.ReadNamespacedAsync(templateNamespace, templateName);
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

            var existingPvcs = await this.K8sClient.CoreV1.ListNamespacedPersistentVolumeClaimAsync(resource.Metadata.NamespaceProperty, labelSelector: $"jupyternetes.kadense.io/instance={resource.Metadata.Name},jupyternetes.kadense.io/instanceNamespace={resource.Metadata.NamespaceProperty}");
            foreach(var Pvc in existingPvcs.Items)
            {
                _logger.LogInformation("Pvc {PvcName} in namespace {Namespace} will not be removed.", Pvc.Metadata.Name, resource.Metadata.NamespaceProperty);
                
                await this.CreateEventAsync(
                    related: resource.ToV1ObjectReference(),
                    action: "Orphaned by Jupyternetes Pvc Operator",
                    reason: "Pvc Orphaned",
                    involvedObject: new k8s.Models.V1ObjectReference(
                        kind: "Pvc",
                        name: Pvc.Metadata.Name,
                        namespaceProperty: resource.Metadata.NamespaceProperty
                    ),
                    message: $"Orphaned by JupyterNotebookInstance/{resource.Metadata.Name} Resource Deletion"
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