namespace Kadense.Models.Kubernetes.CoreApi
{
    public class V1SELinuxOptions : KadenseTemplatedCopiedResource<k8s.Models.V1SELinuxOptions>
    {
        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("level")]
        public string? Level { get; set; }

        public override k8s.Models.V1SELinuxOptions ToOriginal(Dictionary<string, string> variables)
        {
            return new k8s.Models.V1SELinuxOptions(
                user: this.GetValue(this.User, variables),
                role: this.GetValue(this.Role, variables),
                type: this.GetValue(this.Type, variables),
                level: this.GetValue(this.Level, variables)
            );
        }
    }
}