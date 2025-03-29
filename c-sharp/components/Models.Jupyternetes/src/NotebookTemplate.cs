namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplate
    {
        /// <summary>
        /// Name of the template being referenced
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Namespace of the template being referenced
        /// </summary>
        public string? Namespace { get; set; }
    }
}