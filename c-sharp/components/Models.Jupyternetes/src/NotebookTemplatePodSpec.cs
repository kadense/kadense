using Kadense.Models.Kubernetes.CoreApi;
using System.Security.Cryptography;

namespace Kadense.Models.Jupyternetes
{
    public class NotebookTemplatePodSpec : KadenseTemplatedCopiedResource<k8s.Models.V1Pod>
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
        public V1PodSpec? Spec { get; set; }

        public string GeneratePodName(Dictionary<string, string> variables)
        {
            var inputString = $"{variables["template.name"]}-{variables["instance.name"]}";
            var stringBuilder = new System.Text.StringBuilder();
            using(var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputString));
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }
            }
            var podName = $"{stringBuilder.ToString()}-{this.GetValue(this.Name, variables)}";
            return podName;
        }
        public override k8s.Models.V1Pod ToOriginal(Dictionary<string, string> variables)
        {
            var labels = this.GetValue(this.Labels, variables)!;
            var podName = this.GetValue(this.Name, variables)!;
            labels.Add("jupyternetes.kadense.io/podName", podName);

            return new k8s.Models.V1Pod()
            {
                Metadata = new k8s.Models.V1ObjectMeta()
                {
                    Name = this.GeneratePodName(variables),
                    Annotations = this.GetValue(this.Annotations, variables),
                    Labels = labels
                },
                Spec = this.Spec?.ToOriginal(variables)
            };
        }
    }
}