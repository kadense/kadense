namespace Kadense.Models.Malleable
{
    public class MalleableModuleSpec
    {
        /// <summary>
        /// The module description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Classes for this module
        /// </summary>
        [JsonPropertyName("classes")]
        public Dictionary<string, MalleableClass>? Classes { get; set; }
    }
}