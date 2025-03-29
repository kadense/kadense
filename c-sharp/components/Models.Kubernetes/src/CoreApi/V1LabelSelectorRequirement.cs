namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1LabelSelectorRequirement
    {
        public string? Key { get; set; }
        public string? Operator { get; set; }
        public List<string>? Values { get; set; }
    }
}