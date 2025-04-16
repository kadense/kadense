using Kadense.Models.Kubernetes.CoreApi;
using System.Security.Cryptography;
using System.Text.Json;

namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplateOtherSpec
    {
        /// <summary>
        /// If specified, then the pod will only be created if the conditions are met
        /// </summary>
        [JsonPropertyName("condition")]
        public string? Condition { get; set; }

        /// <summary>
        /// Name of the resource
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Api Version of the resource
        /// </summary>
        [JsonPropertyName("apiVersion")]
        public string? ApiVersion { get; set; }

        /// <summary>
        /// Api Version of the resource
        /// </summary>
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }


        /// <summary>
        /// Annotations for the resource
        /// </summary>
        [JsonPropertyName("annotations")]
        public Dictionary<string, string>? Annotations { get; set; }

        /// <summary>
        /// Labels for the resource
        /// </summary>
        [JsonPropertyName("labels")]
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();    

        /// <summary>
        /// The specification for this resource
        /// </summary>
        [JsonPropertyName("spec")]
        [KubernetesAllowAnyValue]
        public object? Spec { get; set; }       
    }
}