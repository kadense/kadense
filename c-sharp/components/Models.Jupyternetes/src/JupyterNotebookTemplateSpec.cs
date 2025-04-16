using k8s.Models;

namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookTemplateSpec
    {
        /// <summary>
        /// List of pod specifications for this template
        /// </summary>
        [JsonPropertyName("pods")]
        public List<NotebookTemplatePodSpec>? Pods { get; set; } = new List<NotebookTemplatePodSpec>();

        [JsonPropertyName("pvcs")]
        public List<NotebookTemplatePvcSpec>? Pvcs { get; set; } = new List<NotebookTemplatePvcSpec>();

        [JsonPropertyName("otherResources")]
        public List<NotebookTemplateOtherSpec>? OtherResources { get; set; } = new List<NotebookTemplateOtherSpec>();

    }
}