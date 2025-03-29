namespace Kadense.Models.Kubernetes
{
    // Attribute to define metadata for Kubernetes custom resources
    public class KubernetesCustomResourceAttribute : Attribute
    {
        // The API group of the custom resource
        public string Group { get; set; } = "{Group}";

        // The kind of the custom resource
        public string Kind { get; set; }

        // The plural name of the custom resource
        public string PluralName { get; set; }

        // The version of the custom resource
        public string Version { get; set; } = "v1";

        // Constructor to initialize the plural name and kind of the custom resource
        public KubernetesCustomResourceAttribute(string pluralName, string kind)
        {
            PluralName = pluralName;
            Kind = kind;
        }
    }
}