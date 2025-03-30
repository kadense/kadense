using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes;
using System.Reflection;

namespace Kadense.Models.Jupyternetes.Tests {
    public class JupyterNotebookTemplateTest
    {
        // Test method to verify the generation of a CustomResourceDefinition YAML
        // for a given Kubernetes object type.
        [Theory]
        [InlineData(new object[] { typeof(JupyterNotebookTemplate) })]
        public async Task GenerateCrdAsync(Type inputType)
        {
            // Create a memory stream to hold the generated YAML.
            using (var stream = new MemoryStream())
            {
                // Call the method under test to generate the YAML.
                await CustomResourceDefinitionBuilder.BuildAsync(stream, inputType);

                // Read the generated YAML from the stream.
                var reader = new StreamReader(stream);
                string yaml = await reader.ReadToEndAsync();
                string expectedYaml = GetEmbeddedResourceAsString("Kadense.Models.Jupyternetes.Tests.crds.JupyterNotebookTemplate.yaml");
                
                // Assert that the generated YAML matches the expected value.
                Assert.Equal(expectedYaml, yaml);

                // Close the stream to release resources.
                stream.Close();
            }
        }

        private string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Resource '{resourceName}' not found.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}