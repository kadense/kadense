using System.Text.Json.Serialization;

namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1Capabilities : KadenseTemplatedCopiedResource<k8s.Models.V1Capabilities>
    { 
        [JsonPropertyName("add")]
        public List<string>? Add { get; set; }

        [JsonPropertyName("drop")]
        public List<string>? Drop { get; set; }

        public override k8s.Models.V1Capabilities ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1Capabilities(
                add: this.GetValue(this.Add, variables),
                drop: this.GetValue(this.Drop, variables)
            );
        }
    }
}