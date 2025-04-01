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
    }
}