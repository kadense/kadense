using System;
using k8s;
using k8s.Models;
using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes
{
    public class KadenseCustomResourceList<T> : KubernetesObject
        where T : KadenseCustomResource
    {
        [JsonPropertyName("metadata")]
        public V1ListMeta Metadata { get; set; } = new V1ListMeta();

        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();
    }
}