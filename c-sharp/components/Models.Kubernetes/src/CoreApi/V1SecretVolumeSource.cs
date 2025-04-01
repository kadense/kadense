namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretVolumeSource : KadenseTemplatedCopiedResource<k8s.Models.V1SecretVolumeSource>
    {
        public string? SecretName { get; set; }
        public bool? Optional { get; set; }
        public int? DefaultMode { get; set; }
        public List<V1KeyToPath>? Items { get; set; }

        public override k8s.Models.V1SecretVolumeSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SecretVolumeSource(
                secretName: this.GetValue(this.SecretName, variables),
                defaultMode: this.DefaultMode,
                optional: this.Optional,
                items: this.GetValue<V1KeyToPath, k8s.Models.V1KeyToPath>(this.Items, variables)
            );
        }
    }
}