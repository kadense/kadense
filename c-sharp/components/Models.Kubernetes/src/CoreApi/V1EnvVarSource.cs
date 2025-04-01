
namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1EnvVarSource : KadenseTemplatedCopiedResource<k8s.Models.V1EnvVarSource>
    {
        public V1ConfigMapKeySelector? ConfigMapKeyRef { get; set; }
        public V1ObjectFieldSelector? FieldRef { get; set; }
        public V1ResourceFieldSelector? ResourceFieldRef { get; set; }
        public V1SecretKeySelector? SecretKeyRef { get; set; }

        public override k8s.Models.V1EnvVarSource ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1EnvVarSource(
                configMapKeyRef : this.ConfigMapKeyRef != null ? this.ConfigMapKeyRef.ToOriginal(variables) : null,
                fieldRef: this.FieldRef != null ?  this.FieldRef.ToOriginal(variables) : null,
                resourceFieldRef: this.ResourceFieldRef != null ? this.ResourceFieldRef.ToOriginal(variables) : null,
                secretKeyRef: this.SecretKeyRef != null ? this.SecretKeyRef.ToOriginal(variables) : null
            );
        }
    }
}