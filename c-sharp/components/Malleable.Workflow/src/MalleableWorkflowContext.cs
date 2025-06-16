using System.Text.Json;
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

        public Dictionary<string, Type> Utils { get; } = new Dictionary<string, Type>();

        public Dictionary<string, MalleableWorkflowStepWithExternalQueue> ExternalSteps { get; } = new Dictionary<string, MalleableWorkflowStepWithExternalQueue>();

        public Dictionary<string, MalleableWorkflowProvider> Providers { get; } = new Dictionary<string, MalleableWorkflowProvider>();

        public void Send<TMessage>(string destination, TMessage message)
            where TMessage : MalleableBase
        {
            if (Destinations.TryGetValue(destination, out var connection))
            {
                connection.Send<TMessage>(message);
            }
            else
            {
                throw new InvalidOperationException($"Destination {destination} not found.");
            }
        }

        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }

        public JsonSerializerOptions GetJsonSerializerOptions()
        {
            if(TypeResolver == null)
            {
                TypeResolver = new MalleablePolymorphicTypeResolver();
                foreach (var assembly in Assemblies)
                {
                    TypeResolver.MalleableAssembly.Add(assembly.Value);
                }
            }
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = TypeResolver,
                WriteIndented = true
            };

            return options;
        }
        
        
    }
}