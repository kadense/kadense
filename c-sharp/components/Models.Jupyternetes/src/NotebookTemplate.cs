namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplate
    {
        /// <summary>
        /// Name of the template being referenced
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Namespace of the template being referenced
        /// </summary>
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }
    }
}