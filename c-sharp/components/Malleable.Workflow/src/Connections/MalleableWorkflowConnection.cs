using System.Text.Json;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Connections
{
    public abstract class MalleableWorkflowConnection
    {
        public abstract void Send<TMessage>(TMessage message);
    }

    public abstract class MalleableWorkflowConnection<T> : MalleableWorkflowConnection
    {
        public MalleableWorkflowConnection(IList<MalleableAssembly> assemblies, T context)
        {
            Assemblies = assemblies;
            Context = context;
        }

        public T Context { get; }

        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }
        public IList<MalleableAssembly> Assemblies { get; }
        public JsonSerializerOptions GetJsonSerializerOptions()
        {
            if(TypeResolver == null)
            {
                TypeResolver = new MalleablePolymorphicTypeResolver();
                foreach (var assembly in Assemblies)
                {
                    TypeResolver.MalleableAssembly.Add(assembly);
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