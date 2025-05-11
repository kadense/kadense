using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;

namespace Kadense.Malleable.Workflow.Connections
{
    public abstract class MalleableWorkflowStepWithExternalQueue
    {
        public MalleableWorkflowStepWithExternalQueue(MalleableWorkflowContext workflowContext, string stepName)
        {
            WorkflowContext = workflowContext;
            StepName = stepName;
            StepDefinition = workflowContext.Workflow.Spec!.Steps![stepName];
        }

        protected MalleableWorkflowContext WorkflowContext { get; }

        protected string StepName { get; set; }

        protected MalleableWorkflowStep StepDefinition { get; set; }  

        
        protected abstract void OnReceive(object message);
        
        public abstract MalleableWorkflowConnection GetConnection();
    }

    public class MalleableWorkflowStepWithExternalQueue<TIn, TOut, TProcessor, TConnection> : MalleableWorkflowStepWithExternalQueue
        where TIn : MalleableBase
        where TOut : MalleableBase
        where TProcessor : MalleableWorkflowProcessor<TIn, TOut>
        where TConnection : MalleableWorkflowConnection, IListeningWorkflowConnection

    {
        public MalleableWorkflowStepWithExternalQueue(MalleableWorkflowContext workflowContext, string stepName, TConnection connection) : base(workflowContext, stepName)
        {
            Processor = (TProcessor)Activator.CreateInstance(typeof(TProcessor), new object[] { this.WorkflowContext, stepName })!;
            Connection = connection;
            Connection.OnReceive = OnReceive;
            Connection.Listen<TIn>();
        }

        protected TProcessor Processor { get; }

        protected TConnection Connection { get; }

        public override MalleableWorkflowConnection GetConnection()
        {
            return Connection;
        }

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
        }
    }
}