using System.Diagnostics;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public abstract class MalleableWorkflowProcessor
    {
        public abstract (string?, MalleableBase) Process(MalleableBase message);

        public string ProcessorSignature { get; set; } = string.Empty;
    }
    public abstract class MalleableWorkflowProcessor<TIn, TOut> : MalleableWorkflowProcessor
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public MalleableWorkflowProcessor(MalleableWorkflowContext context, string stepName)
        {
            Context = context;
            StepName = stepName;
            StepDefinition = context.Workflow.Spec!.Steps![stepName];
        }

        public MalleableWorkflowContext Context { get; set; }

        public string StepName { get; set; } = string.Empty;
        protected MalleableWorkflowStep StepDefinition { get; set; }

        public abstract string? GetErrorDestination();
    }
}