namespace Kadense.Models.Kubernetes
{
    public class KubernetesLabelSelector : Dictionary<string, string>
    {
        public override string ToString()
        {
            return string.Join(",", this.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}