namespace Kadense.Models.Malleable
{
    [KubernetesCustomResource("MalleableModules", "MalleableModule", HasStatusField = true)]
    [KubernetesCategoryName("malleable")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("mmod")]
    public class MalleableModule : KadenseCustomResource
    {
        [JsonPropertyName("spec")]
        public MalleableModuleSpec? Spec { get; set; }
    }
}