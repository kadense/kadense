namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1PreferredSchedulingTerm
    {
        public int? Weight { get; set; }
        public V1NodeSelectorTerm? Preference { get; set; }
    }
}