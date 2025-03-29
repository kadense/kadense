using Kadense.Models.Kubernetes;

namespace Kadense.Models.Kubernetes.Tests {
    public class CustomResourceDefinitionBuilderTest
    {
        // Test method to verify the generation of a CustomResourceDefinition YAML
        // for a given Kubernetes object type.
        [Theory]
        [InlineData(new object[] { typeof(TestKubernetesObject) })]
        public async Task GenerateAsync(Type inputType)
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
                Assert.Equal("", yaml);

                // Close the stream to release resources.
                stream.Close();
            }
        }
    }
}