using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Extensions
{
    public static class ActorSystemExtensions
    {
        public static MalleableWorkflowCoordinatorFactory AddWorkflow(this ActorSystem sys, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies)
        {
            return new MalleableWorkflowCoordinatorFactory(sys, workflow, assemblies);
        }
        public static MalleableWorkflowCoordinatorFactory AddWorkflow(this ActorSystem sys, MalleableWorkflow workflow, MalleableAssemblyFactory assemblyFactory)
        {
            return new MalleableWorkflowCoordinatorFactory(sys, workflow, assemblyFactory.GetAssemblies());
        }
    }
}