using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableConverterReference : IMalleableValidated, IMalleableHasModules
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
        [JsonPropertyName("converterName")]
        public string? ConverterName { get; set; }

        public string GetQualifiedModuleName()
        {
            return $"{ModuleNamespace}:{ModuleName}";
        }

        public string GetQualifiedConverterName()
        {
            return $"{ModuleNamespace}:{ModuleName}:{ConverterName}";
        }

        public bool IsValid(ILogger logger)
        {
            if (string.IsNullOrEmpty(ModuleName) || string.IsNullOrEmpty(ModuleNamespace) || string.IsNullOrEmpty(ConverterName))
            {
                logger.LogError("ConverterReference is missing required fields");
                return false;
            }
            return true;
        }

        public IEnumerable<string> GetModuleNames()
        {
            var items = new List<string>();
            items.Add(GetQualifiedModuleName());
            return items;
        }
    }
}