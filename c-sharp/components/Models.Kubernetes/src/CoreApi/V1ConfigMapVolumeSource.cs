namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1ConfigMapVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1ConfigMapVolumeSource>
    {
        public string? Name { get; set; }
        public bool? Optional { get; set; }
        public int? DefaultMode { get; set; }
        public List<V1KeyToPath>? Items { get; set; }

        public override k8s.Models.V1ConfigMapVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1ConfigMapVolumeSource(
                name: this.GetValue(this.Name, variables),
                optional: this.Optional,
                defaultMode: this.DefaultMode,
                items: this.Items != null ? this.Items.Select(i => i.ToOriginal(variables)).ToList() : null
            );
        }
    }
}