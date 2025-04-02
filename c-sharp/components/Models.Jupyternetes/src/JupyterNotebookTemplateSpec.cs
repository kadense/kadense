using k8s.Models;

namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookTemplateSpec
    {
        /// <summary>
        /// List of pod specifications for this template
        /// </summary>
        [JsonPropertyName("pods")]
        public List<NotebookTemplatePodSpec>? Pods { get; set; }
    }
}