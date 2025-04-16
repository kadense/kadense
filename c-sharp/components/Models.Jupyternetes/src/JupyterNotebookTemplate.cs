using k8s.Models;

namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResource("JupyterNotebookTemplates", "JupyterNotebookTemplate")]
    [KubernetesCategoryName("jupyternetes")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("jnt")]
    public class JupyterNotebookTemplate : KadenseCustomResource
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        [JsonPropertyName("spec")]
        public JupyterNotebookTemplateSpec? Spec { get; set; }

        public virtual void UpdateInstantVariablesByStatus(JupyterNotebookInstance instance)
        {
            instance.Spec!.Variables!["template.name"] = this.Metadata.Name!;
            instance.Spec!.Variables!["template.namespace"] = this.Metadata.NamespaceProperty!;
            instance.Spec!.Variables!["instance.name"] = instance.Metadata.Name!;
            instance.Spec!.Variables!["instance.namespace"] = instance.Metadata.NamespaceProperty!;
            
            foreach(var pvcStatus in instance.Status.Pvcs)
            {
                if(pvcStatus.Value.ResourceName != null)
                {
                    instance.Spec!.Variables![$"jupyternetes.pvcs.{pvcStatus.Key}"] = pvcStatus.Value.ResourceName!;
                }
            }

            foreach(var podStatus in instance.Status.Pods)
            {
                if(podStatus.Value.ResourceName != null)
                {
                    instance.Spec!.Variables![$"jupyternetes.pods.{podStatus.Key}"] = podStatus.Value.ResourceName!;
                }
            }
        }

        public virtual (IList<k8s.Models.V1Pod>, IDictionary<string, Exception>) CreatePods(JupyterNotebookInstance instance)
        {
            UpdateInstantVariablesByStatus(instance);

            var conversionIssues = new Dictionary<string, Exception>();
            var pods = new List<k8s.Models.V1Pod>();
            foreach (var podSpec in this.Spec!.Pods!)
            {
                try 
                {
                    var pod = podSpec.ToOriginal(instance.Spec!.Variables!);
                    pod.Metadata.Labels["jupyternetes.kadense.io/instance"] = instance.Metadata.Name!;
                    pod.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"] = instance.Metadata.NamespaceProperty!;
                    pod.Metadata.Labels["jupyternetes.kadense.io/template"] = this.Metadata.Name!;
                    pod.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"] = this.Metadata.NamespaceProperty!;
                    pods.Add(pod);
                    instance.Spec!.Variables![$"jupyternetes.pods.{podSpec.Name}"] = pod.Metadata!.Name!;
                }
                catch(AwaitingDependencyException adEx)
                {
                    conversionIssues.Add(podSpec.Name!, adEx);
                }
                catch(VariableNotPopulatedException vnpEx)
                {
                    conversionIssues.Add(podSpec.Name!, vnpEx);
                }
            }
            return (pods, conversionIssues);
        }

        
        public virtual (IList<k8s.Models.V1PersistentVolumeClaim>, IDictionary<string, Exception>) CreatePvcs(JupyterNotebookInstance instance)
        {
            UpdateInstantVariablesByStatus(instance);
            
            var conversionIssues = new Dictionary<string, Exception>();
            var pvcs = new List<k8s.Models.V1PersistentVolumeClaim>();
            foreach (var pvcSpec in this.Spec!.Pvcs!)
            {
                try
                {
                    var pvc = pvcSpec.ToOriginal(instance.Spec!.Variables!);
                    pvc.Metadata.Labels["jupyternetes.kadense.io/instance"] = instance.Metadata.Name!;
                    pvc.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"] = instance.Metadata.NamespaceProperty!;
                    pvc.Metadata.Labels["jupyternetes.kadense.io/template"] = this.Metadata.Name!;
                    pvc.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"] = this.Metadata.NamespaceProperty!;
                    pvcs.Add(pvc);
                }
                catch(AwaitingDependencyException adEx)
                {
                    conversionIssues.Add(pvcSpec.Name!, adEx);
                }
                catch(VariableNotPopulatedException vnpEx)
                {
                    conversionIssues.Add(pvcSpec.Name!, vnpEx);
                }
            }
            return (pvcs, conversionIssues);
        }
    }
}