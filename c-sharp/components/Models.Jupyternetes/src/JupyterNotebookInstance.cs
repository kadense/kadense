namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResource("JupyterNotebookInstances", "JupyterNotebookInstance")]
    [KubernetesCategoryName("jupyternetes")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("jni")]
    public class JupyterNotebookInstance : KadenseCustomResource
    {
        /// <summary>
        /// Specification for this instance
        /// </summary>
        [JsonPropertyName("spec")]
        public JupyterNotebookInstanceSpec? Spec { get; set; }
    }
}