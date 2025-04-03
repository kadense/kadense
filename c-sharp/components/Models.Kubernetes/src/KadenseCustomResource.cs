using System;
using k8s;
using k8s.Models;
using System.Text.Json.Serialization;
using System.Reflection;

namespace Kadense.Models.Kubernetes
{
    public class KadenseCustomResource : KubernetesObject, IMetadata<V1ObjectMeta>
    {
        [IgnoreOnCrdGeneration]
        [JsonPropertyName("metadata")]
        public V1ObjectMeta Metadata { get; set; }

        public KadenseCustomResource()
        {
            this.Metadata = new V1ObjectMeta();
            var attribute = this.GetType().GetCustomAttributes<KubernetesCustomResourceAttribute>().First();
            this.ApiVersion = $"{attribute.Group}/{attribute.Version}";
            this.Kind = attribute.Kind;
        }

        public k8s.Models.V1ObjectReference ToV1ObjectReference()
        {
            return new k8s.Models.V1ObjectReference
            {
                ApiVersion = this.ApiVersion,
                Kind = this.Kind,
                Name = this.Metadata.Name,
                NamespaceProperty = this.Metadata.NamespaceProperty,
                ResourceVersion = this.Metadata.ResourceVersion
            };
        }
    }
}