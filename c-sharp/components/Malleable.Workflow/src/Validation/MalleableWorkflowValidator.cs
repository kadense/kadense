using System.Diagnostics;
using Kadense.Malleable.Reflection;
using Microsoft.Extensions.Logging;

namespace Kadense.Malleable.Workflow.Validation
{
    public class MalleableWorkflowValidator
    {
        public static MalleableWorkflowValidator CreateDefault(MalleableWorkflowCoordinatorFactory factory, ILogger logger)
        {
            return new MalleableWorkflowValidator(factory, "BasicValidation", (ctx) => {
                if(ctx.Workflow == null)
                {
                    logger.LogError("Workflow is null");
                    return false;
                }
                if(ctx.Workflow.Spec == null)
                {
                    logger.LogError("Spec is null");
                    return false;
                }
                return ctx.Workflow.Spec.IsValid(logger);
            }, logger)
            .AddNext("TypeValidation", (ctx) => {
                return WorkflowTypeValidation.Validate(ctx, logger);   
            });
        }

        public MalleableWorkflowValidator(MalleableWorkflowCoordinatorFactory factory, string name, Func<MalleableWorkflowContext, bool> actionFunction, ILogger logger)
        {
            Name = name;
            Factory = factory;
            ActionFunction = actionFunction;
            Logger = logger;
        }

        public MalleableWorkflowValidator(MalleableWorkflowCoordinatorFactory factory, string name, Func<MalleableWorkflowContext, bool> actionFunction, ILogger logger, MalleableWorkflowValidator previous) : this(factory, name, actionFunction, logger)
        {
            Previous = previous;
        }

        private ILogger Logger { get; }

        private MalleableWorkflowCoordinatorFactory Factory { get; }

        private string Name { get; }

        private Func<MalleableWorkflowContext, bool> ActionFunction { get; }

        private MalleableWorkflowValidator? Previous { get; set; }
        private MalleableWorkflowValidator? Next { get; set; }

        public MalleableWorkflowValidator AddNext(string name, Func<MalleableWorkflowContext, bool> actionFunction)
        {
            var next = new MalleableWorkflowValidator(Factory, name, actionFunction, Logger, this);
            Next = next;
            return next;
        }

        public MalleableWorkflowCoordinatorFactory CompleteValidations()
        {
            return Factory;
        }

        public bool Validate(MalleableWorkflowContext context)
        {
            var isValid = ActionFunction(context);
            if (isValid)
            {
                if (Next != null)
                {
                    return Next.Validate(context);
                }
                return true;
            }
            else
            {
                Logger.LogError($"Validation failed for {Name}");
                return false;
            }
        }

        public MalleableWorkflowValidator GetFirst()
        {
            if (Previous != null)
            {
                return Previous.GetFirst();
            }
            return this;
        }
    }
}