namespace Kadense.Models.Malleable
{
    public class MalleableTypeReference
    {
        /// <summary>
        /// The name of the module that this type is defined in
        /// </summary>
        [JsonPropertyName("moduleName")]
        public string? ModuleName { get; set; }

        /// <summary>
        /// The namespace of the module that this type is defined in
        /// </summary>
        [JsonPropertyName("moduleNamespace")]
        public string? ModuleNamespace { get; set; }

        /// <summary>
        /// The name of the class being referenced
        /// </summary>
        [JsonPropertyName("className")]
        public string? ClassName { get; set; }

        public string GetQualifiedModuleName()
        {
            return $"{ModuleNamespace}:{ModuleName}";
        }
        public string GetQualifiedClassName()
        {
            return $"{ModuleNamespace}:{ModuleName}:{ClassName}";
        }
    }
}