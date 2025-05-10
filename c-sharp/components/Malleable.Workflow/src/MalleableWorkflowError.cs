using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow
{
    public abstract class MalleableWorkflowError : MalleableBase
    {

    }

    public class MalleableWorkflowError<T> : MalleableWorkflowError
        where T : MalleableBase
    {
        public MalleableWorkflowError()
        {
            
        }
        public MalleableWorkflowError(T message, Exception error)
        {
            Message = message;
            Error = error;
        }

        public Exception? Error { get; set; }

        public T? Message { get; set; }
    }
}