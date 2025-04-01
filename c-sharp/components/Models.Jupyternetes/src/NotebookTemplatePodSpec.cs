using Kadense.Models.Kubernetes.CoreApi;

namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplatePodSpec
    {
        /// <summary>
        /// If specified, then the pod will only be created if the conditions are met
        /// </summary>
        [JsonPropertyName("condition")]
        public string? Condition { get; set; }

        /// <summary>
        /// Name of the pod
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Annotations for the pod
        /// </summary>
        [JsonPropertyName("annotations")]
        public Dictionary<string, string>? Annotations { get; set; }

        /// <summary>
        /// Labels for the pod
        /// </summary>
        [JsonPropertyName("labels")]
        public Dictionary<string, string>? Labels { get; set; }    

        /// <summary>
        /// The pod specification for this template
        /// </summary>
        [JsonPropertyName("spec")]
        public V1PodSpec? Spec { get; set; }
    }
}