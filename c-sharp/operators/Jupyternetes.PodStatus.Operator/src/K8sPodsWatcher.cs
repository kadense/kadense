using Kadense.Client.Kubernetes;
using k8s;
using k8s.Models;
using k8s.Exceptions;
using Kadense.Logging;
using System.Security.Cryptography.X509Certificates;
using Kadense.Models.Jupyternetes;

namespace Kadense.Jupyternetes.PodStatus.Operator
{
    public class K8sPodsWatcher : KadenseResourceWatcher<V1Pod>
    {
        public KadenseCustomResourceClient<JupyterNotebookInstance> InstanceClient { get; set; }
        private KadenseLogger<K8sPodsWatcher> Logger { get; set; }

        public bool IsRunning { get; set; } = false;
        public K8sPodsWatcher() : base()
        {
            // Initialize the watcher
            this.Logger = new KadenseLogger<K8sPodsWatcher>();
            var clientFactory = new CustomResourceClientFactory();
            this.InstanceClient = clientFactory.Create<JupyterNotebookInstance>(this.K8sClient);
            this.GenericClient = new GenericClient(this.K8sClient, "v1", "pods");
        }

        public override async Task<(V1Pod?, Corev1Event?)> OnAddedAsync(V1Pod item)
        {
            this.Logger.LogInformation($"Pod {item.Metadata.Name} on {item.Metadata.NamespaceProperty} added");
            return await ProcessPodEvent(item);
        }

        public override async Task<(V1Pod?, Corev1Event?)> OnUpdatedAsync(V1Pod item)
        {
            this.Logger.LogInformation($"Pod {item.Metadata.Name} on {item.Metadata.NamespaceProperty} updated");
            return await ProcessPodEvent(item);
        }

        public override Task<(V1Pod?, Corev1Event?)> OnDeletedAsync(V1Pod item)
        {
            this.Logger.LogInformation($"Pod {item.Metadata.Name} on {item.Metadata.NamespaceProperty} deleted");
            return Task.FromResult<(V1Pod?, Corev1Event?)>((item, null));
        }

        private async Task UpdateStatusAsync(JupyterNotebookInstance resource)
        {
            await this.K8sClient.CustomObjects.PatchNamespacedCustomObjectStatusAsync(
                body: new k8s.Models.V1Patch(
                    new Dictionary<string, object>()
                    {
                        { "status", resource.Status }
                    },
                    type: V1Patch.PatchType.MergePatch
                ),
                group: "kadense.io",
                version: "v1",
                plural: "jupyternotebookinstances",
                namespaceParameter: resource.Metadata.NamespaceProperty!,
                name: resource.Metadata.Name!
            );
            this.Logger.LogInformation("Patched Status {ResourceName}.", resource.Metadata.Name);
        }

        protected virtual async Task<Corev1Event> CreateEventAsync(V1ObjectMeta involvedObject, string action, string reason, Corev1EventSeries? series = null, V1Pod? related = null, string type = "Normal", string? message = null)
        {
            return await CreateEventAsync(
                involvedObject: new V1ObjectReference(
                    apiVersion: "kadense.io/v1",
                    kind: "JupyterNotebookInstance",
                    name: involvedObject.Name,
                    namespaceProperty: involvedObject.NamespaceProperty,
                    uid: involvedObject.Uid,
                    resourceVersion: involvedObject.ResourceVersion
                ),
                action: action,
                reason: reason,
                series: series,
                related: new V1ObjectReference(
                    apiVersion: "v1",
                    kind: "Pod",
                    name: related?.Metadata.Name,
                    namespaceProperty: related?.Metadata.NamespaceProperty,
                    uid: related?.Metadata.Uid,
                    resourceVersion: related?.Metadata.ResourceVersion
                ),
                type: type,
                message: message
            );
        }

        public async Task UpdateNotebookInstanceAsync(V1Pod pod, string instanceName, string instanceNamespace, string podName, string podIP)
        {
            this.Logger.LogInformation($"Checking Notebook Instance {instanceName} in {instanceName} to see if {podName} has IP {podIP}");
            var notebookInstance = await this.InstanceClient.ReadNamespacedAsync(instanceNamespace, instanceName);
            if (notebookInstance != null)
            {
                this.Logger.LogInformation($"Notebook Instance {notebookInstance.Metadata.Name} in namespace {notebookInstance.Metadata.NamespaceProperty} is found");
                if (notebookInstance.Status == null)
                {
                    notebookInstance.Status = new JupyterNotebookInstanceStatus();
                }
                if (!notebookInstance.Status.Pods.ContainsKey(podName))
                {
                    this.Logger.LogWarning($"Notebook Instance {notebookInstance.Metadata.Name} in namespace {notebookInstance.Metadata.NamespaceProperty} does not have pod status for {podName}");
                    return;
                }
                bool updated = false;

                if (notebookInstance.Status.Pods[podName].PodAddress == null || notebookInstance.Status.Pods[podName].PodAddress != podIP)
                {
                    this.Logger.LogInformation($"Notebook Instance {notebookInstance.Metadata.Name} in namespace {notebookInstance.Metadata.NamespaceProperty}, pod {podName} will be updated with PodAddress = {podIP}");
                    notebookInstance.Status.Pods[podName].PodAddress = podIP;
                    await this.CreateEventAsync(notebookInstance.Metadata, "PodAddressUpdated", "PodAddressUpdated", series: null, related: pod, type: "Normal", message: $"Pod {podName} address updated to {podIP}");
                    updated = true;
                }

                if (notebookInstance.Status.Pods[podName].State == null || notebookInstance.Status.Pods[podName].State != "Running")
                {
                    this.Logger.LogInformation($"Notebook Instance {notebookInstance.Metadata.Name} in namespace {notebookInstance.Metadata.NamespaceProperty}, pod {podName} will be updated to State of Running");
                    notebookInstance.Status.Pods[podName].State = "Running";
                    await this.CreateEventAsync(notebookInstance.Metadata, "PodStateUpdated", "PodIsRunning", series: null, related: pod, type: "Normal", message: $"Pod state on pod {podName} is updated to Running");
                    updated = true;
                }

                if (updated)
                {
                    this.Logger.LogInformation($"Updating Notebook Instance {notebookInstance.Metadata.Name} in namespace {notebookInstance.Metadata.NamespaceProperty} with pod status for {podName}");
                    await UpdateStatusAsync(notebookInstance);
                }
            }
            else
            {
                this.Logger.LogWarning($"Notebook Instance {instanceName} in namespace {instanceNamespace} not found");
            }
        }

        public async Task<(V1Pod?, Corev1Event?)> ProcessPodEvent(V1Pod pod)
        {
            this.Logger.LogInformation($"Processing Pod Event: {pod.Metadata.Name}");
            if (pod.Metadata.Labels.TryGetValue("jupyternetes.kadense.io/instance", out string? instanceName))
            {
                if (pod.Metadata.Labels.TryGetValue("jupyternetes.kadense.io/instanceNamespace", out string? instanceNamespace))
                {
                    if (pod.Metadata.Labels.TryGetValue("jupyternetes.kadense.io/podName", out string? podName))
                    {
                        this.Logger.LogInformation($"Pod {pod.Metadata.Name} in {pod.Metadata.NamespaceProperty} is using instance {instanceName} in namespace {instanceNamespace}");
                        var podStatus = pod.Status;
                        if (podStatus != null)
                        {
                            if (podStatus.Phase == "Running")
                            {
                                this.Logger.LogInformation($"Pod {pod.Metadata.Name} in {pod.Metadata.NamespaceProperty} is Running");
                                if (podStatus.PodIP != null)
                                {
                                    await UpdateNotebookInstanceAsync(pod, instanceName, instanceNamespace, podName, podStatus.PodIP);
                                }
                            }
                        }
                    }
                }
            }
            return (pod, null);
        }
    }
}