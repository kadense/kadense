using System.Diagnostics;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;

namespace Kadense.Malleable.Workflow
{
    public abstract class MalleableWorkflowActor : UntypedActor
    {
        public MalleableWorkflowActor(MalleableWorkflowContext workflowContext, string stepName)
        {
            WorkflowContext = workflowContext;
            StepName = stepName;
            StepDefinition = workflowContext.Workflow.Spec!.Steps![stepName];
        }

        protected MalleableWorkflowContext WorkflowContext { get; }

        protected string StepName { get; set; }

        protected MalleableWorkflowStep StepDefinition { get; set; }
    }
    public class MalleableWorkflowActor<TIn,TOut,TProcessor> : MalleableWorkflowActor
        where TIn : MalleableBase 
        where TOut : MalleableBase
        where TProcessor : MalleableWorkflowProcessor<TIn, TOut> 
    {
        public MalleableWorkflowActor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            Processor = (TProcessor)Activator.CreateInstance(typeof(TProcessor), new object[] { this.WorkflowContext, stepName })!;
        }

        protected TProcessor Processor { get; }

        protected override void OnReceive(object message)
        {
            if (message is TIn input)
            {
                try 
                {
                    (string? destination, MalleableBase result) = Processor.Process(input);
                    if(result is not TOut processedResult)
                        throw new InvalidOperationException($"Invalid result type. Expected {typeof(TOut)}, but got {result.GetType()}.");
                    
                    if(!String.IsNullOrEmpty(destination))
                        this.WorkflowContext.Send(destination, processedResult);

                }
                catch(Exception ex)
                {
                    var errorDestination = Processor.GetErrorDestination();

                    if (errorDestination == null && !this.WorkflowContext.DebugMode)
                        throw;

                    var errorMessage = new MalleableWorkflowError<TIn>(input, ex);
                    if (errorDestination != null)
                    {
                        this.WorkflowContext.Send(errorDestination, errorMessage);
                    }
                }
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}