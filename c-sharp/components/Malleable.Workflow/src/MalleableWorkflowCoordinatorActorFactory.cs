using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Malleable.Workflow.Connections;
using k8s.Models;
using Kadense.Malleable.Workflow.Validation;
using Kadense.Logging;
using Microsoft.Extensions.Logging;
using Kadense.Malleable.Workflow.Apis;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowCoordinatorActorFactory 
    {

        public MalleableWorkflowCoordinatorActorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies)
        {
            Logger = new KadenseLogger<MalleableWorkflowCoordinatorActor>();
            System = system;
            Workflow = workflow;
            ActionMapper = MalleableWorkflowAction.CreateDefault(this);
            ValidatorMapper = MalleableWorkflowValidator.CreateDefault(this, Logger);
            ApiActionMapper = MalleableWorkflowApiAction.CreateDefault(this);
            WorkflowContext = new MalleableWorkflowContext(Workflow, assemblies, false);
        }

        public MalleableWorkflowCoordinatorActorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies, ILogger logger) : this(system, workflow, assemblies)
        {
            Logger = logger;
        }

        public ILogger Logger { get; } 
        public ActorSystem System { get; }
        protected MalleableWorkflowValidator ValidatorMapper { get; set; } 
        protected MalleableWorkflowAction ActionMapper { get; set;}
        protected MalleableWorkflowApiAction ApiActionMapper { get; set; } 
        public MalleableWorkflow Workflow { get; } 

        public MalleableWorkflowValidator GetValidators() => ValidatorMapper.GetFirst();
        public MalleableWorkflowAction GetActions() => ActionMapper.GetFirst();
        public MalleableWorkflowApiAction GetApiActions() => ApiActionMapper.GetFirst();

        public MalleableWorkflowCoordinatorActorFactory WithDebugMode(bool debugMode = true)
        {
            this.WorkflowContext.DebugMode = debugMode;
            return this;
        }

        public MalleableWorkflowContext WorkflowContext { get; protected set; }

        public IActorRef Build()
        {
            var actorProps = Props.Create<MalleableWorkflowCoordinatorActor>(new object[] { WorkflowContext, GetActions(), GetValidators() });
            return System.ActorOf(actorProps, Workflow.Metadata.Name);
        }
    }
}