namespace Kadense.Models.Malleable;

public class MalleableWorkflowFactory
{
    public MalleableWorkflowFactory(string @namespace, string name)
    {
        Workflow = new MalleableWorkflow
        {
            Metadata = new k8s.Models.V1ObjectMeta
            {
                NamespaceProperty = @namespace,
                Name = name
            },
            Spec = new MalleableWorkflowSpec
            {
                APIs = new Dictionary<string, MalleableWorkflowApi>(),
                Steps = new Dictionary<string, MalleableWorkflowStep>()
            }
        };
    }

    public MalleableWorkflow Workflow { get; }

    public MalleableWorkflowStepFactory<MalleableWorkflowFactory> AddStep(string name, string action, Dictionary<string, string>? parameters = null)
    {
        var stepFactory = new MalleableWorkflowStepFactory<MalleableWorkflowFactory>(this, name, action, parameters, this);
        Workflow.Spec!.Steps!.Add(name, stepFactory.Step);
        return stepFactory;
    }

    public MalleableWorkflowApiFactory AddApi(string name, string apiType = "Ingress")
    {
        var apiFactory = new MalleableWorkflowApiFactory(this, apiType);
        Workflow.Spec!.APIs!.Add(name, apiFactory.Api);
        return apiFactory;
    }

    public MalleableWorkflow EndWorkflow()
    {
        return Workflow;
    }
}
