using System.Diagnostics;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public class MalleableWorkflowAction
    {
        public static MalleableWorkflowAction CreateDefault(MalleableWorkflowCoordinatorFactory factory)
        {
            return new MalleableWorkflowAction(factory, "Convert", (ctx, stepName) => {
                var step = ctx.Workflow.Spec!.Steps![stepName];
                var moduleName = step.ConverterOptions!.Converter!.GetQualifiedModuleName();
                var converterName = step.ConverterOptions!.Converter!.ConverterName;
                var converterType = ctx.Assemblies[moduleName].Types[converterName!];
                var converterAttribute = MalleableConverterAttribute.FromType(converterType);
                var convertFromType = ctx.Assemblies[converterAttribute.GetConvertFromModuleName()].Types[converterAttribute.ConvertFromClassName];
                var convertToType = ctx.Assemblies[converterAttribute.GetConvertToModuleName()].Types[converterAttribute.ConvertToClassName]; 
                var processorType = typeof(ConversionProcessor<,>).MakeGenericType(new Type[] { convertFromType, convertToType }); 
                return processorType;
            })
            .AddNext("IfElse", (ctx, stepName) => {
                var inputType = ctx.StepInputTypes[stepName];
                
                var processorType = typeof(IfElseProcessor<>).MakeGenericType(new Type[] { inputType }); 
                
                return processorType;
            })
            .AddNext("WriteApi", (ctx, stepName) => {
                var step = ctx.Workflow.Spec!.Steps![stepName];
                var inputType = ctx.StepInputTypes[stepName];
                Type? outputType = null;
                if(step.Options != null)
                {
                    if(step.Options.OutputType != null)
                    {
                        var outputTypeName = step.Options.OutputType.GetQualifiedModuleName();
                        outputType = ctx.Assemblies[outputTypeName].Types[step.Options.OutputType.ClassName!];
                    }
                }
                if (outputType == null)
                    outputType = inputType;
                
                var processorType = typeof(ApiWriteProcessor<,>).MakeGenericType(new Type[] { inputType, outputType }); 
                
                return processorType;
            });
        }
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, string action, Func<MalleableWorkflowContext, string, Type> actionFunction)
        {
            Action = action;
            ActionFunction = actionFunction;
            Factory = factory;
        }
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, string action, Func<MalleableWorkflowContext, string, Type> actionFunction, MalleableWorkflowAction previous) : this(factory, action, actionFunction)
        {
            Previous = previous;
        }

        private MalleableWorkflowCoordinatorFactory Factory { get; }

        private string Action { get; }

        private Func<MalleableWorkflowContext, string, Type > ActionFunction { get; }

        private MalleableWorkflowAction? Previous { get; set; }
        private MalleableWorkflowAction? Next { get; set; }

        public MalleableWorkflowAction AddNext(string action, Func<MalleableWorkflowContext, string, Type> actionFunction)
        {
            var next = new MalleableWorkflowAction(Factory, action, actionFunction, this);
            Next = next;
            return next;
        }

        public MalleableWorkflowCoordinatorFactory CompleteActions()
        {
            return Factory;
        }

        public Type? Create(MalleableWorkflowContext context, string stepName)
        {
            if(context.Workflow.Spec!.Steps![stepName].Action == Action)
            {
                var type = ActionFunction(context, stepName);
                if (type != null)
                    return type;

                throw new InvalidOperationException($"Action function for step {stepName} returned null.");
            }
            else if(Next != null)
            {
                return Next.Create(context, stepName);
            }

            throw new InvalidOperationException($"No action found for step {stepName}.");
        }

        public MalleableWorkflowAction GetFirst()
        {
            if (Previous != null)
            {
                return Previous.GetFirst();
            }
            return this;
        }

    }
}