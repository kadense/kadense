using System.Diagnostics;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public class MalleableWorkflowAction
    {
        public static MalleableWorkflowAction CreateDefault(MalleableWorkflowCoordinatorActorFactory factory)
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
                var actorType = typeof(MalleableWorkflowActor<,,>).MakeGenericType(new Type[] { convertFromType, convertToType, processorType });
                return Props.Create(actorType, new object[] { ctx, stepName });
            })
            .AddNext("IfElse", (ctx, stepName) => {
                var inputType = ctx.StepInputTypes[stepName];
                
                var processorType = typeof(IfElseProcessor<>).MakeGenericType(new Type[] { inputType }); 
                var actorType = typeof(MalleableWorkflowActor<,,>).MakeGenericType(new Type[] { inputType, inputType, processorType });
                
                return Props.Create(actorType, new object[] { ctx, stepName });
            });
        }
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorActorFactory factory, string action, Func<MalleableWorkflowContext, string, Props> actionFunction)
        {
            Action = action;
            ActionFunction = actionFunction;
            Factory = factory;
        }
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorActorFactory factory, string action, Func<MalleableWorkflowContext, string, Props> actionFunction, MalleableWorkflowAction previous) : this(factory, action, actionFunction)
        {
            Previous = previous;
        }

        private MalleableWorkflowCoordinatorActorFactory Factory { get; }

        private string Action { get; }

        private Func<MalleableWorkflowContext, string, Props?> ActionFunction { get; }

        private MalleableWorkflowAction? Previous { get; set; }
        private MalleableWorkflowAction? Next { get; set; }

        public MalleableWorkflowAction AddNext(string action, Func<MalleableWorkflowContext, string, Props> actionFunction)
        {
            var next = new MalleableWorkflowAction(Factory, action, actionFunction, this);
            Next = next;
            return next;
        }

        public MalleableWorkflowCoordinatorActorFactory CompleteActions()
        {
            return Factory;
        }

        public Props? CreateProps(MalleableWorkflowContext context, string stepName)
        {
            if(context.Workflow.Spec!.Steps![stepName].Action == Action)
            {
                var props = ActionFunction(context, stepName);
                if (props != null)
                    return props;

                throw new InvalidOperationException($"Action function for step {stepName} returned null.");
            }
            else if(Next != null)
            {
                return Next.CreateProps(context, stepName);
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