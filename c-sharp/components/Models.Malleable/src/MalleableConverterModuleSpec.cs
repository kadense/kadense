namespace Kadense.Models.Malleable
{
    public class MalleableConverterModuleSpec
    {
        /// <summary>
        /// The various converters that are available in this module
        /// </summary>
        [JsonPropertyName("converters")]
        public Dictionary<string, MalleableTypeConverter> Converters { get; set; } = new Dictionary<string, MalleableTypeConverter>();
    }
}