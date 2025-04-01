
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SecretKeySelector : KadenseTemplatedCopiedResource<k8s.Models.V1SecretKeySelector>
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
        public bool? Optional { get; set; }

        public override k8s.Models.V1SecretKeySelector ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SecretKeySelector(
                key: this.GetValue(this.Key, variables),
                name: this.GetValue(this.Name, variables),
                optional: this.Optional
            );
        }
    }
}