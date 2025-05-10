namespace Kadense.Models.Malleable
{
    [KubernetesCustomResource("MalleableWorkflows", "MalleableWorkflow", HasStatusField = true)]
    [KubernetesCategoryName("malleable")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("mwf")]
    public class MalleableWorkflow : KadenseCustomResource
    {
        [JsonPropertyName("spec")]
        public MalleableWorkflowSpec? Spec { get; set; }
    }
}