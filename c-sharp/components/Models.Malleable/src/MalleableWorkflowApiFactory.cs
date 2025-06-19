namespace Kadense.Models.Malleable;

public class MalleableWorkflowApiFactory
{
    public MalleableWorkflowApiFactory(MalleableWorkflowFactory workflowFactory, string apiType)
    {
        WorkflowFactory = workflowFactory;
        Api = new MalleableWorkflowApi()
        {
            ApiType = apiType,
            IngressOptions = new MalleableWorkflowApiOptions(),
        };
    }

    public MalleableWorkflowFactory WorkflowFactory { get; }

    public MalleableWorkflowApi Api { get; }

    public MalleableWorkflowApiFactory SetUnderlyingType(string @namespace, string moduleName, string className)
    {
        Api.UnderlyingType = new MalleableTypeReference()
        {
            ClassName = className,
            ModuleName = moduleName,
            ModuleNamespace = @namespace
        };
        return this;
    }

    public MalleableWorkflowApiFactory SetUnderlyingType(MalleableTypeReference typeReference)
    {
        Api.UnderlyingType = typeReference;
        return this;
    }

    public MalleableWorkflowStepFactory<MalleableWorkflowApiFactory> AddStep(string name, string action, Dictionary<string, string>? parameters = null)
    {
        if (Api.ApiType == "Ingress")
        {
            Api.IngressOptions!.NextStep = name;
        }
        var stepFactory = new MalleableWorkflowStepFactory<MalleableWorkflowApiFactory>(this.WorkflowFactory, name, action, parameters, this);
        WorkflowFactory.Workflow.Spec!.Steps!.Add(name, stepFactory.Step);
        return stepFactory;
    }

    public MalleableWorkflowApiFactory AddStep(string name)
    {
        if (Api.ApiType == "Ingress")
        {
            Api.IngressOptions!.NextStep = name;
        }
        return this;
    }


    public MalleableWorkflowFactory EndApi()
    {
        return WorkflowFactory;
    }
}
