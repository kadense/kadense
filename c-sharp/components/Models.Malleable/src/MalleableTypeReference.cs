namespace Kadense.Models.Malleable
{
    public class MalleableTypeReference
    {
        /// <summary>
        /// The description of the converter
        /// </summary>
        [JsonPropertyName("typeName")]
        public string? TypeName { get; set; }

        /// <summary>
        /// If this is a collection type, the type of the collection
        /// </summary>
        [JsonPropertyName("subTypeName")]
        public string? SubTypeName { get; set; }
    }
}