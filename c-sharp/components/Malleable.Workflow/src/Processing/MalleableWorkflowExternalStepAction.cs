using System.Diagnostics;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;

namespace Kadense.Malleable.Workflow.Processing
{
    public class MalleableWorkflowExternalStepAction
    {
        public static MalleableWorkflowExternalStepAction CreateDefault(MalleableWorkflowCoordinatorFactory factory)
        {
            return new MalleableWorkflowExternalStepAction(factory, "{Default}", (ctx, stepName) => {
                throw new InvalidOperationException($"No action found for step {stepName}.");
            });
        }
        public MalleableWorkflowExternalStepAction(MalleableWorkflowCoordinatorFactory factory, string executorType, Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue?> actionFunction)
        {
            ExecutorType = executorType;
            ActionFunction = actionFunction;
            Factory = factory;
        }
        public MalleableWorkflowExternalStepAction(MalleableWorkflowCoordinatorFactory factory, string executorType, Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue?> actionFunction, MalleableWorkflowExternalStepAction previous) : this(factory, executorType, actionFunction)
        {
            Previous = previous;
        }

        private MalleableWorkflowCoordinatorFactory Factory { get; }

        private string ExecutorType { get; }

        private Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue?> ActionFunction { get; }

        private MalleableWorkflowExternalStepAction? Previous { get; set; }
        private MalleableWorkflowExternalStepAction? Next { get; set; }

        public MalleableWorkflowExternalStepAction AddNext(string executorType, Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue?> actionFunction)
        {
            var next = new MalleableWorkflowExternalStepAction(Factory, executorType, actionFunction, this);
            Next = next;
            return next;
        }

        public MalleableWorkflowCoordinatorFactory CompleteActions()
        {
            return Factory;
        }

        public MalleableWorkflowStepWithExternalQueue? Create(MalleableWorkflowContext context, string stepName)
        {
            if(context.Workflow.Spec!.Steps![stepName].ExecutorType == ExecutorType)
            {
                var result = ActionFunction(context, stepName);
                if (result != null)
                    return result;

                throw new InvalidOperationException($"Action function for step {stepName} returned null.");
            }
            else if(Next != null)
            {
                return Next.Create(context, stepName);
            }

            throw new InvalidOperationException($"No action found for step {stepName}.");
        }

        public MalleableWorkflowExternalStepAction GetFirst()
        {
            if (Previous != null)
            {
                return Previous.GetFirst();
            }
            return this;
        }

    }
}