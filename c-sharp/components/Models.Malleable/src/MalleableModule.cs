namespace Kadense.Models.Malleable
{
    [KubernetesCustomResource("MalleableModules", "MalleableModule", HasStatusField = true)]
    [KubernetesCategoryName("malleable")]
    [KubernetesCategoryName("kadense")]
    [KubernetesShortName("mmod")]
    public class MalleableModule : KadenseCustomResource, IComparable
    {
        [JsonPropertyName("spec")]
        public MalleableModuleSpec? Spec { get; set; }


        public int CompareTo(object? obj)
        {
            if (obj is MalleableModule other)
            {
                if(Spec == null || other.Spec == null)
                    return 0;
                if (Spec.Classes == null || other.Spec.Classes == null)
                    return 0;
                if (Spec.Classes.Count == 0 || other.Spec.Classes.Count == 0)
                    return 0;
                
                return Spec.GetReferencedModules().Contains(this.GetQualifiedModuleName()) ? -1 : 1;
            }
            return 0;
        }

        public string GetQualifiedModuleName()
        {
            return $"{Metadata?.NamespaceProperty ?? String.Empty}:{Metadata?.Name ?? String.Empty}";
        }

        public override string ToString()
        {
            return GetQualifiedModuleName();
        }
    }
}