using System.Diagnostics;
using System.Reflection;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public class MalleableWorkflowAction
    {
        public static MalleableWorkflowAction CreateDefault(MalleableWorkflowCoordinatorFactory factory)
        {
            return new MalleableWorkflowAction(factory, typeof(ConversionProcessor<,>))
            .AddNext(typeof(IfElseProcessor<>))
            .AddNext(typeof(ApiWriteProcessor<,>));
        }

        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, Type baseType)
        {
            Factory = factory;
            ProcessorAttribute = baseType.GetCustomAttribute<MalleableWorkflowProcessorAttribute>(true)!; 
            Action = ProcessorAttribute.ActionName;
        }

        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, string actionName, Type baseType)
        {
            Factory = factory;
            ProcessorAttribute = baseType.GetCustomAttribute<MalleableWorkflowProcessorAttribute>(true)!; 
            Action = actionName;
        }
        
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, Type baseType, MalleableWorkflowAction previous) : this(factory, baseType)
        {
            Previous = previous;
        }
        
        public MalleableWorkflowAction(MalleableWorkflowCoordinatorFactory factory, string actionName, Type baseType, MalleableWorkflowAction previous) : this(factory, actionName, baseType)
        {
            Previous = previous;
        }

        private MalleableWorkflowCoordinatorFactory Factory { get; }

        private string Action { get; }

        private MalleableWorkflowProcessorAttribute ProcessorAttribute { get; }


        private MalleableWorkflowAction? Previous { get; set; }
        private MalleableWorkflowAction? Next { get; set; }

        public MalleableWorkflowAction AddNext(Type baseType)
        {
            var next = new MalleableWorkflowAction(Factory, baseType, this);
            Next = next;
            return next;
        }
        public MalleableWorkflowAction AddNext(string actionName, Type baseType)
        {
            var next = new MalleableWorkflowAction(Factory, actionName, baseType, this);
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
                var type = ProcessorAttribute.CreateType(context, stepName);
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