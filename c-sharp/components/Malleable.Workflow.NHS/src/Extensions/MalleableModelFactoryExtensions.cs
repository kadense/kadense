using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.NHS.Extensions
{
    public static class MalleableModelFactoryExtensions
    {
        public static MalleableModule FromFhirSchema(this MalleableModuleFactory factory, string namespacePrefix, string moduleName)
        {
            string jsonSchema = GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.NHS.Resources.Fhir.STU3.fhir.schema.json");
            var module = factory.FromFhirSchema(jsonSchema, namespacePrefix, moduleName);
            return module;
        }

        private static string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = typeof(MalleableModelFactoryExtensions).Assembly;
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}