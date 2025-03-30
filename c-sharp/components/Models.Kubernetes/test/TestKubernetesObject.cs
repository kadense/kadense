using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.Tests
{

    [KubernetesCustomResourceAttribute("kadensetests", "kadensetest")]
    public class TestKubernetesObject
    {
        // The name of the Kubernetes object, serialized as "myName".
        [JsonPropertyName("myName")]
        public string Name { get; set; }

        // An integer value associated with the object.
        public int Value { get; set; }

        // An enumeration representing specific states or types.
        public TestEnum ValueEnum { get; set; }

        // A child object containing additional details.
        public TestChildObject ValueChildObject { get; set; }

        // Default constructor to initialize non-nullable properties.
        public TestKubernetesObject()
        {
            Name = string.Empty;
            ValueChildObject = new TestChildObject();
        }

        // Enumeration for predefined values.
        public enum TestEnum
        {
            // Represents a test state.
            Testing123,
            // Represents an "OMG" state.
            OMG,
            // Represents a "WHY" state.
            WHY
        }
    }
}