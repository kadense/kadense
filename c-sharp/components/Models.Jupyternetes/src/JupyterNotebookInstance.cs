namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResourceAttribute("JupyterNotebookInstances", "JupyterNotebookInstance")]
    public class JupyterNotebookInstance
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        public JupyterNotebookInstanceSpec? Spec { get; set; }
    }
}