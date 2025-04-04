namespace Kadense.Models.Jupyternetes
{
    [KubernetesCustomResource("JupyterNotebookInstances", "JupyterNotebookInstance", HasStatusField = true)]
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

        [JsonPropertyName("status")]
        public JupyterNotebookInstanceStatus Status { get; set; } = new JupyterNotebookInstanceStatus();
    }
}