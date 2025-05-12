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
            MalleableWorkflowQueueProcessor.OnReceive<TIn, TOut, TProcessor>(message, WorkflowContext, Processor, StepName);
        }
    }
}