namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookInstanceSpec
    {
        /// <summary>
        /// The notebook template object for this Instance
        /// </summary>
        [JsonPropertyName("template")]
        public NotebookTemplate? Template { get; set; }

        /// <summary>
        /// Variables to be passed to the template for this Instance
        /// </summary>
        [JsonPropertyName("variables")]
        public Dictionary<string, string>? Variables { get;set; }
    }
}