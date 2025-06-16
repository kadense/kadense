namespace Kadense.Malleable.Workflow.Connections;

public class ActorConnectionOptions : MalleableWorkflowConnectionOptions
{
    public ActorConnectionOptions(IActorRef context)
    {
        Context = context;
    }
    public IActorRef Context { get; set; }
}
