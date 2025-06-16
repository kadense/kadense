namespace Kadense.Models.Malleable;

public class MalleableWorkflowStepFactory<TReturn>
{
    public MalleableWorkflowStepFactory(MalleableWorkflowFactory workflowFactory, string name, string action, Dictionary<string, string>? parameters, TReturn parent)
    {
        WorkflowFactory = workflowFactory;
        Name = name;
        Parent = parent;
        Step = new MalleableWorkflowStep
        {
            Action = action,
            Options = new MalleableWorkflowStandardActionOptions
            {
                Parameters = parameters ?? new Dictionary<string, string>()
            }
        };
    }

    public MalleableWorkflowFactory WorkflowFactory { get; }
    public MalleableWorkflowStep Step { get; }
    public TReturn Parent { get; }
    public string Name { get; }
    public MalleableWorkflowStepFactory<TReturn> AddNextStep(string name)
    {
        Step.Options!.NextStep = name;
        return this;
    }

    public MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>> AddNextStep(string name, string action, Dictionary<string, string>? parameters = null)
    {
        AddNextStep(name);
        var nextStep = new MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>>(WorkflowFactory, name, action, parameters, this);
        WorkflowFactory.Workflow.Spec!.Steps!.Add(name, nextStep.Step);
        return nextStep;
    }

    public MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>>? AddErrorStep(string name, string? action = null, Dictionary<string, string>? parameters = null)
    {
        Step.Options!.ErrorStep = name;

        if (action == null)
            return null;

        var errorStep = new MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>>(WorkflowFactory, name, action, parameters, this);
        WorkflowFactory.Workflow.Spec!.Steps!.Add(name, errorStep.Step);
        return errorStep;
    }

    public MalleableWorkflowStepFactory<TReturn> SetConverter(string @namespace, string moduleName, string converterName)
    {
        if (Step.ConverterOptions == null)
            Step.ConverterOptions = new MalleableWorkflowConverterActionOptions();

        Step.ConverterOptions.Converter = new MalleableConverterReference
        {
            ConverterName = converterName,
            ModuleName = moduleName,
            ModuleNamespace = @namespace
        };
        return this;
    }

    public MalleableWorkflowStepFactory<TReturn> SetConverter(MalleableConverterReference converter)
    {
        if (Step.ConverterOptions == null)
            Step.ConverterOptions = new MalleableWorkflowConverterActionOptions();

        Step.ConverterOptions.Converter = converter;
        return this;
    }

    public MalleableWorkflowStepFactory<TReturn> AddIfCondition(string conditionExpression, string name)
    {
        if (Step.IfElseOptions == null)
            Step.IfElseOptions = new MalleableWorkflowIfElseActionOptions()
            {
                Expressions = new List<MalleableWorkflowIfElseExpression>()
            };

        Step.IfElseOptions.Expressions.Add(new MalleableWorkflowIfElseExpression
        {
            Expression = conditionExpression,
            NextStep = name
        });

        return this;
    }
    

    public MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>> AddIfCondition(string conditionExpression, string name, string action, Dictionary<string, string>? parameters = null)
    {
        AddIfCondition(conditionExpression, name);        
        var conditionalStep = new MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>>(WorkflowFactory, name, action, parameters, this);
        WorkflowFactory.Workflow.Spec!.Steps!.Add(name, conditionalStep.Step);
        return conditionalStep;
    }

    public MalleableWorkflowStepFactory<TReturn> SetElse(string name)
    {
        if (Step.IfElseOptions == null)
            Step.IfElseOptions = new MalleableWorkflowIfElseActionOptions()
            {
                Expressions = new List<MalleableWorkflowIfElseExpression>()
            };

        Step.IfElseOptions.ElseNextStep = name;
        return this;
    }


    public MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>> SetElse(string name, string action, Dictionary<string, string>? parameters = null)
    {
        SetElse(name);
        var conditionalStep = new MalleableWorkflowStepFactory<MalleableWorkflowStepFactory<TReturn>>(WorkflowFactory, name, action, parameters, this);
        WorkflowFactory.Workflow.Spec!.Steps!.Add(name, conditionalStep.Step);
        return conditionalStep;
    }

    public TReturn EndStep()
    {
        return this.Parent;
    }

    public MalleableWorkflowStepFactory<TReturn> SetParameter(string name, string value)
    {
        if (Step.Options == null)
            Step.Options = new MalleableWorkflowStandardActionOptions();

        if (Step.Options.Parameters == null)
            Step.Options.Parameters = new Dictionary<string, string>();

        Step.Options.Parameters[name] = value;
        return this;
    }
}
