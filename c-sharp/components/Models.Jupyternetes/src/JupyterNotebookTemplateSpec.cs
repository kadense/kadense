namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookTemplateSpec
    {
        /// <summary>
        /// List of pod specifications for this template
        /// </summary>
        public List<NotebookTemplatePodSpec>? Pods { get; set; }
    }
}