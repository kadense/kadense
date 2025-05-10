using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Extensions
{
    public static class ActorSystemExtensions
    {
        public static  MalleableWorkflowCoordinatorActorFactory AddWorkflow(this ActorSystem sys, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies)
        {
            return new MalleableWorkflowCoordinatorActorFactory(sys, workflow, assemblies);
        }
    }
}