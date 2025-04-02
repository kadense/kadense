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

        public virtual IList<k8s.Models.V1Pod> CreatePods(JupyterNotebookInstance instance)
        {
            instance.Spec!.Variables!["template.name"] = this.Metadata.Name!;
            instance.Spec!.Variables!["template.namespace"] = this.Metadata.NamespaceProperty!;
            instance.Spec!.Variables!["instance.name"] = instance.Metadata.Name!;
            instance.Spec!.Variables!["instance.namespace"] = instance.Metadata.NamespaceProperty!;
            
            var pods = new List<k8s.Models.V1Pod>();
            foreach (var podSpec in this.Spec!.Pods!)
            {
                var pod = podSpec.ToOriginal(instance.Spec!.Variables!);
                pod.Metadata.Labels["jupyternetes.kadense.io/instance"] = instance.Metadata.Name!;
                pod.Metadata.Labels["jupyternetes.kadense.io/instanceNamespace"] = instance.Metadata.NamespaceProperty!;
                pod.Metadata.Labels["jupyternetes.kadense.io/template"] = this.Metadata.Name!;
                pod.Metadata.Labels["jupyternetes.kadense.io/templateNamespace"] = this.Metadata.NamespaceProperty!;
                pods.Add(pod);
            }
            return pods;
        }
    }
}