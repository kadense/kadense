using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public interface IListeningWorkflowConnection
    {
        public void Listen<TMessage>()
            where TMessage : MalleableBase;

        public Delegate OnReceive { get; set; } 

    }
}