using Kadense.Models.Kubernetes.CoreApi;
using System.Security.Cryptography;

namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplatePvcSpec : KadenseTemplatedCopiedResource<k8s.Models.V1PersistentVolumeClaim>
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
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();    

        /// <summary>
        /// The pod specification for this template
        /// </summary>
        [JsonPropertyName("spec")]
        public V1PersistentVolumeClaimSpec? Spec { get; set; }

        public override k8s.Models.V1PersistentVolumeClaim ToOriginal(Dictionary<string, string> variables)
        {
            var labels = this.GetValue(this.Labels, variables)!;
            var pvcName = this.GetValue(this.Name, variables)!;
            labels.Add("jupyternetes.kadense.io/pvcName", pvcName);

            return new k8s.Models.V1PersistentVolumeClaim()
            {
                Metadata = new k8s.Models.V1ObjectMeta()
                {
                    GenerateName = $"{pvcName}-",
                    Annotations = this.GetValue(this.Annotations, variables),
                    Labels = labels
                },
                Spec = this.Spec?.ToOriginal(variables)
            };
        }
    }
}