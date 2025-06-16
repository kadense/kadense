using System.Text.Json;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public abstract class MalleableWorkflowConnection
    {
        public abstract void Initialize(MalleableWorkflowContext workflowContext, string stepName);
        public abstract void Send<TMessage>(TMessage message);
    }

    public abstract class MalleableWorkflowConnection<T> : MalleableWorkflowConnection
        where T : MalleableWorkflowConnectionOptions
    {
        public MalleableWorkflowConnection(MalleableWorkflowContext workflowContext, T options)
        {
            WorkflowContext = workflowContext;
            Options = options;
        }

        public T Options { get; }

        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }
        public MalleableWorkflowContext WorkflowContext { get; }
        public JsonSerializerOptions GetJsonSerializerOptions()
        {
            if (TypeResolver == null)
            {
                TypeResolver = new MalleablePolymorphicTypeResolver();
                foreach (var assembly in WorkflowContext.Assemblies)
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