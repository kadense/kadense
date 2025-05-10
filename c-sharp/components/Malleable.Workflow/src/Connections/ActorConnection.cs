using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public class ActorConnection : MalleableWorkflowConnection<IActorRef>
    {
        public ActorConnection(IActorRef context) : base(context)
        {

        }

        public override void Send<TMessage>(TMessage message)
        {
            Context.Tell(message);
        }
    }
}