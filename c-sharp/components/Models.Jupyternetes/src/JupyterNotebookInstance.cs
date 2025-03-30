namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResourceAttribute("JupyterNotebookInstances", "JupyterNotebookInstance")]
    [KubernetesCategoryName("jupyternetes")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("jni")]
    public class JupyterNotebookInstance
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        public JupyterNotebookInstanceSpec? Spec { get; set; }
    }
}