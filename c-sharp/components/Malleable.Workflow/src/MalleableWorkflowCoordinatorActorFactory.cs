using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Malleable.Workflow.Connections;
using k8s.Models;
using Kadense.Malleable.Workflow.Validation;
using Kadense.Logging;
using Microsoft.Extensions.Logging;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowCoordinatorActorFactory 
    {

        public MalleableWorkflowCoordinatorActorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies)
        {
            Logger = new KadenseLogger<MalleableWorkflowCoordinatorActor>();
            System = system;
            Workflow = workflow;
            Assemblies = assemblies;
            ActionMapper = MalleableWorkflowAction.CreateDefault(this);
            ValidatorMapper = MalleableWorkflowValidator.CreateDefault(this, Logger);
            WorkflowContext = new MalleableWorkflowContext(Workflow, Assemblies, false);
        }

        public MalleableWorkflowCoordinatorActorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies, ILogger logger) : this(system, workflow, assemblies)
        {
            Logger = logger;
        }

        public ILogger Logger { get; } 
        public ActorSystem System { get; }
        public MalleableWorkflowValidator ValidatorMapper { get; } 
        public MalleableWorkflowAction ActionMapper { get; } 
        public MalleableWorkflow Workflow { get; } 
        public IDictionary<string, MalleableAssembly> Assemblies { get; }


        public MalleableWorkflowCoordinatorActorFactory WithDebugMode(bool debugMode = true)
        {
            this.WorkflowContext.DebugMode = debugMode;
            return this;
        }

        public MalleableWorkflowContext WorkflowContext { get; protected set; }

        public IActorRef Build()
        {
            var actorProps = Props.Create<MalleableWorkflowCoordinatorActor>(new object[] { WorkflowContext, ActionMapper.GetFirst(), ValidatorMapper.GetFirst() });
            return System.ActorOf(actorProps, Workflow.Metadata.Name);
        }
    }
}