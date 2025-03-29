using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes;

namespace Kadense.Models.Jupyternetes.Tests {
    public class CustomResourceDefinitionBuilderTest
    {
        // Test method to verify the generation of a CustomResourceDefinition YAML
        // for a given Kubernetes object type.
        [Theory]
        [InlineData(new object[] { typeof(JupyterNotebookInstance) })]
        public async Task GenerateJupyterNotebookInstanceAsync(Type inputType)
        {
            // Create a memory stream to hold the generated YAML.
            using (var stream = new MemoryStream())
            {
                // Call the method under test to generate the YAML.
                await CustomResourceDefinitionBuilder.BuildAsync(stream, inputType);

                // Read the generated YAML from the stream.
                var reader = new StreamReader(stream);
                string yaml = await reader.ReadToEndAsync();

                // Assert that the generated YAML matches the expected value.
                Assert.Equal("apiVersion: apiextensions.k8s.io/v1\nkind: CustomResourceDefinition\nmetadata:\n  name: jupyternotebookinstances.kadense.io\nspec:\n  group: kadense.io\n  versions:\n  - name: v1\n    served: true\n    storage: true\n    schema:\n      openAPIV3Schema:\n        type: object:\n        properties:\n          spec:\n            type: object\n            properties:\n              spec:\n                type: object\n                properties:\n                  template:\n                    type: object\n                    properties:\n                      name:\n                        type: string\n                      namespace:\n                        type: string\n                  variables:\n                    type: object\n                    additionalProperties:\n                      type: string\n", yaml);

                // Close the stream to release resources.
                stream.Close();
            }
        }
    }
}