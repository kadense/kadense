using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public abstract class MalleableWorkflowConnection
    {
        public abstract void Send<TMessage>(TMessage message);
    }

    public abstract class MalleableWorkflowConnection<T> : MalleableWorkflowConnection
    {
        public MalleableWorkflowConnection(T context)
        {
            Context = context;
        }

        public T Context { get; }

    }
}