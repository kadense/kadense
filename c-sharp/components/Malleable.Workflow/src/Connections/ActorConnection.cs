using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public class ActorConnection : MalleableWorkflowConnection<IActorRef>
    {
        public ActorConnection(IList<MalleableAssembly> assemblies, IActorRef context) : base(assemblies, context)
        {

        }

        public override void Send<TMessage>(TMessage message)
        {
            Context.Tell(message);
        }
    }
}