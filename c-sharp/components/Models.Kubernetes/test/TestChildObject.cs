namespace Kadense.Models.Kubernetes.Tests
{
    /// <summary>
    /// Represents a test object with basic properties for testing purposes.
    /// </summary>
    public class TestChildObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestChildObject"/> class.
        /// </summary>
        public TestChildObject()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name of the test object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the test object.
        /// </summary>
        public string Description { get; set; }
    }
}
