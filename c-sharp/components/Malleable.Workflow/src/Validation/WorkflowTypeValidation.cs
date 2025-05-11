using Kadense.Malleable.Reflection;
using Microsoft.Extensions.Logging;

namespace Kadense.Malleable.Workflow.Validation;

public static class WorkflowTypeValidation
{
    public static bool ValidateStep(MalleableWorkflowContext ctx, string name, Type lastType, ILogger logger)
    {
        var step = ctx.Workflow.Spec!.Steps![name];
        switch(step.Action)
        {
            case "Convert":
                var moduleName = step.ConverterOptions!.Converter!.GetQualifiedModuleName();
                var converterName = step.ConverterOptions!.Converter!.ConverterName;
                var converterType = ctx.Assemblies[moduleName].Types[converterName!];
                var converterAttribute = MalleableConverterAttribute.FromType(converterType);
                var convertFromModuleName = converterAttribute.GetConvertFromModuleName();
                var convertFromClassName = converterAttribute.ConvertFromClassName;
                var fromType = ctx.Assemblies[convertFromModuleName].Types[convertFromClassName!];
                if(fromType != lastType)
                {
                    logger.LogError($"Type {lastType} is not valid for converter {converterName} which expects {fromType}");
                    return false;
                }
                var convertToModuleName = converterAttribute.GetConvertToModuleName();
                var convertToClassName = converterAttribute.ConvertToClassName;
                var newType = ctx.Assemblies[convertToModuleName].Types[convertToClassName!];
                ctx.StepInputTypes.Add(name, newType);
                if(step.ConverterOptions.NextStep != null)
                {
                    if (!ctx.StepInputTypes.ContainsKey(step.ConverterOptions.NextStep!))
                    {
                        if(!ValidateStep(ctx, step.ConverterOptions.NextStep!, newType, logger))
                        {
                            logger.LogError($"Step {step.ConverterOptions.NextStep} is invalid");
                            return false;
                        }
                    }
                }
                break;

            case "IfElse":
                foreach(var expression in step.IfElseOptions!.Expressions)
                {
                    if(expression.NextStep != null)
                    {
                        if (!ctx.StepInputTypes.ContainsKey(expression.NextStep!))
                        {
                            ctx.StepInputTypes.Add(name, lastType);

                            if(!ValidateStep(ctx, expression.NextStep!, lastType, logger))
                            {
                                logger.LogError($"Step {expression.NextStep} is invalid");
                                return false;
                            }
                        }
                    }
                }
                break;
            
            default:
                if(step.Options != null)
                {
                    if(step.Options.InputType != null)
                    {
                        var inputType = ctx.Assemblies[step.Options.InputType.GetQualifiedModuleName()].Types[step.Options.InputType.ClassName!];
                        if(inputType != lastType)
                        {
                            logger.LogError($"Type {lastType} is not valid for step {name} which expects {inputType}");
                            return false;
                        }
                    }
                    ctx.StepInputTypes.Add(name, lastType);

                    if(step.Options.OutputType != null)
                    {
                        var outputType = ctx.Assemblies[step.Options.OutputType.GetQualifiedModuleName()].Types[step.Options.OutputType.ClassName!];
                        if(step.Options.NextStep != null)
                        {
                            if(!ValidateStep(ctx, step.Options.NextStep!, outputType, logger))
                            {
                                logger.LogError($"Step {step.Options.NextStep} is invalid");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if(step.Options.NextStep != null)
                        {
                            if(!ValidateStep(ctx, step.Options.NextStep!, lastType, logger))
                            {
                                logger.LogError($"Step {step.Options.NextStep} is invalid");
                                return false;
                            }
                        }
                    }
                    if(step.Options.ErrorStep != null)
                    {
                        if(!ValidateStep(ctx, step.Options.ErrorStep, lastType, logger))
                        {
                            logger.LogError($"Step {step.Options.ErrorStep} is invalid");
                            return false;
                        }
                    }
                }
                else 
                {
                    ctx.StepInputTypes.Add(name, lastType);
                }
                break;
        }
        return true;
    }


    public static bool ValidateApi(MalleableWorkflowContext ctx, string name, MalleableWorkflowApi api, ILogger logger)
    {
        if (api.UnderlyingType == null)
        {
            logger.LogError($"UnderlyingType is null for {name}");
            return false;
        }
      
        var type = ctx.Assemblies[api.UnderlyingType.GetQualifiedModuleName()].Types[api.UnderlyingType!.ClassName!];
        if (type == null)
        {
            logger.LogError($"Type {api.UnderlyingType.GetQualifiedModuleName()}.{api.UnderlyingType.ClassName} not found");
            return false;
        }
      
        switch (api.ApiType)
        {
            case "Ingress":
                if (api.IngressOptions == null)
                {
                    logger.LogError($"IngressOptions is null for {name}");
                    return false;
                }
                if (!ctx.StepInputTypes.ContainsKey(api.IngressOptions.NextStep!))
                {
                    ValidateStep(ctx, api.IngressOptions.NextStep!, type, logger);
                }
                break;
      
            default:
                logger.LogError($"Unknown ApiType {api.ApiType}");
                return false;
        }

        return true;
    }
    
    public static bool Validate(MalleableWorkflowContext ctx, ILogger logger)
    {
        if (ctx.Workflow.Spec!.Steps == null)
        {
            logger.LogError("Steps is null");
            return false;
        }
        if (ctx.Workflow.Spec!.APIs != null)
        {
            foreach (var api in ctx.Workflow.Spec.APIs)
            {
                if(!ValidateApi(ctx, api.Key, api.Value, logger))
                {
                    logger.LogError($"API {api.Key} is invalid");
                    return false;
                }
            }
        }

        foreach (var step in ctx.Workflow.Spec.Steps)
        {
            if (!ctx.StepInputTypes.ContainsKey(step.Key))
            {
                logger.LogError($"Could not determine input type of {step.Key}");
                return false;
            }
        }
        return true;
    }
}
