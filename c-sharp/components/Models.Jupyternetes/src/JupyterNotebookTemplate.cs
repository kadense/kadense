namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResourceAttribute("JupyterNotebookInstanceTemplates", "JupyterNotebookInstanceTemplate")]
    public class JupyterNotebookTemplate
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        public JupyterNotebookTemplateSpec? Spec { get; set; }
    }
}