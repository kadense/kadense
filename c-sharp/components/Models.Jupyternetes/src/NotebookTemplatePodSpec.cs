using Kadense.Models.Kubernetes.CoreApi;

namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplatePodSpec
    {
        /// <summary>
        /// If specified, then the pod will only be created if the conditions are met
        /// </summary>
        public string? Condition { get; set; }

        /// <summary>
        /// Name of the pod
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Annotations for the pod
        /// </summary>
        public Dictionary<string, string>? Annotations { get; set; }

        /// <summary>
        /// Labels for the pod
        /// </summary>
        public Dictionary<string, string>? Labels { get; set; }    

        /// <summary>
        /// The pod specification for this template
        /// </summary>
        public V1PodSpec? Spec { get; set; }
    }
}