namespace Kadense.Models.Malleable
{
    [KubernetesCustomResource("MalleableConverterModules", "MalleableConverterModule", HasStatusField = true)]
    [KubernetesCategoryName("malleable")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("mcm")]
    public class MalleableConverterModule : KadenseCustomResource
    {
        [JsonPropertyName("spec")]
        public MalleableConverterModuleSpec? Spec { get; set; }
    }
}