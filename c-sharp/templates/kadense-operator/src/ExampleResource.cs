using Kadense.Models.Kubernetes;

namespace Kadense.Template.Operator
{
    [KubernetesCustomResource("ExampleResources", "ExampleResource")]
    public class ExampleResource : KadenseCustomResource
    {
        public ExampleResourceSpec? Spec { get; set; }
    }

    public class ExampleResourceSpec
    {
        public string? Test { get;set; }
    }
}