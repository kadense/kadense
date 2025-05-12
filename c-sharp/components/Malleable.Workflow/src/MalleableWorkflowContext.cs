using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowContext
    {
        public MalleableWorkflowContext(MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies, bool debugMode)
        {
            Assemblies = assemblies;
            Workflow = workflow;
            Destinations = new Dictionary<string, MalleableWorkflowConnection>();
            DebugMode = debugMode;
        }

        public bool DebugMode { get; set; }

        public bool EnableMessageSigning { get; set; } = true;

        public MalleableWorkflow Workflow { get; } 
        public IDictionary<string, MalleableWorkflowConnection> Destinations { get; }

        public IDictionary<string, MalleableAssembly> Assemblies { get; }

        public Dictionary<string, Type> StepInputTypes { get; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> StepOutputTypes { get; } = new Dictionary<string, Type>();

        public Dictionary<string, MalleableWorkflowStepWithExternalQueue> ExternalSteps { get; } = new Dictionary<string, MalleableWorkflowStepWithExternalQueue>();

        public void Send<TMessage>(string destination, TMessage message)
        {
            if (Destinations.TryGetValue(destination, out var connection))
            {
                connection.Send(message);
            }
            else
            {
                throw new InvalidOperationException($"Destination {destination} not found.");
            }
        }
    }
}