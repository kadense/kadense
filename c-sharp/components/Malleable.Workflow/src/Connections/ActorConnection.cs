using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public class ActorConnection : MalleableWorkflowConnection<ActorConnectionOptions>
    {
        public ActorConnection(MalleableWorkflowContext workflowContext, ActorConnectionOptions options) : base(workflowContext, options)
        {
            // nothing to do
        }

        public override void Initialize(MalleableWorkflowContext workflowContext, string stepName)
        {
            // nothing to do
        }

        public override void Send<TMessage>(TMessage message)
        {
            Options.Context.Tell(message);
        }
    }
}