namespace Kadense.Models.Jupyternetes
{
    public class JupyterNotebookTemplateSpec
    {
        /// <summary>
        /// List of pod specifications for this template
        /// </summary>
        [JsonPropertyName("pods")]
        public List<NotebookTemplatePodSpec>? Pods { get; set; }
        
        public virtual IList<k8s.Models.V1Pod> CreatePods(JupyterNotebookInstance instance)
        {
            var pods = new List<k8s.Models.V1Pod>();
            foreach (var podSpec in this.Pods!)
            {
                var pod = podSpec.ToOriginal(instance.Spec!.Variables!);

                pods.Add(pod);
            }
            return pods;
        }
    }
}