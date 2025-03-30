namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResource("JupyterNotebookTemplates", "JupyterNotebookTemplate")]
    [KubernetesCategoryName("jupyternetes")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("jnt")]
    public class JupyterNotebookTemplate
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        public JupyterNotebookTemplateSpec? Spec { get; set; }
    }
}