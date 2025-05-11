using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Malleable.Workflow.Connections;
using k8s.Models;
using Kadense.Malleable.Workflow.Validation;
using Kadense.Logging;
using Microsoft.Extensions.Logging;
using Kadense.Malleable.Workflow.Apis;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowCoordinatorFactory 
    {

        public MalleableWorkflowCoordinatorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies)
        {
            Logger = new KadenseLogger<MalleableWorkflowCoordinatorActor>();
            System = system;
            Workflow = workflow;
            ActionMapper = MalleableWorkflowAction.CreateDefault(this);
            ValidatorMapper = MalleableWorkflowValidator.CreateDefault(this, Logger);
            ApiActionMapper = MalleableWorkflowApiAction.CreateDefault(this);
            ExternalStepActionMapper = MalleableWorkflowExternalStepAction.CreateDefault(this);
            WorkflowContext = new MalleableWorkflowContext(Workflow, assemblies, false);
        }

        public MalleableWorkflowCoordinatorFactory(ActorSystem system, MalleableWorkflow workflow, IDictionary<string, MalleableAssembly> assemblies, ILogger logger) : this(system, workflow, assemblies)
        {
            Logger = logger;
        }

        public ILogger Logger { get; } 
        public ActorSystem System { get; }
        protected MalleableWorkflowValidator ValidatorMapper { get; set; } 
        protected MalleableWorkflowAction ActionMapper { get; set;}
        protected MalleableWorkflowApiAction ApiActionMapper { get; set; } 
        protected MalleableWorkflowExternalStepAction ExternalStepActionMapper { get; set; }
        public MalleableWorkflow Workflow { get; } 
        public MalleableWorkflowExternalStepAction GetExternalStepActions() => ExternalStepActionMapper.GetFirst();
        public MalleableWorkflowValidator GetValidators() => ValidatorMapper.GetFirst();
        public MalleableWorkflowAction GetActions() => ActionMapper.GetFirst();
        public MalleableWorkflowApiAction GetApiActions() => ApiActionMapper.GetFirst();

        public MalleableWorkflowValidator AddValidator(string name, Func<MalleableWorkflowContext, bool> actionFunction)
        {
            ValidatorMapper = ValidatorMapper.AddNext(name, actionFunction);
            return ValidatorMapper;
        }
        public MalleableWorkflowAction AddAction(string name, Func<MalleableWorkflowContext, string, Type> actionFunction)
        {
            ActionMapper = ActionMapper.AddNext(name, actionFunction);
            return ActionMapper;
        }

        public MalleableWorkflowApiAction AddApiAction(string name, Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> actionFunction)
        {
            ApiActionMapper = ApiActionMapper.AddNext(name, actionFunction);
            return ApiActionMapper;
        }

        public MalleableWorkflowExternalStepAction AddExternalStepAction(string name, Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue> actionFunction)
        {
            ExternalStepActionMapper = ExternalStepActionMapper.AddNext(name, actionFunction);
            return ExternalStepActionMapper;
        }

        public MalleableWorkflowCoordinatorFactory WithDebugMode(bool debugMode = true)
        {
            this.WorkflowContext.DebugMode = debugMode;
            return this;
        }

        public MalleableWorkflowCoordinatorFactory Validate()
        {
            var isValid = GetValidators().Validate(WorkflowContext);
            if (!isValid)
            {
                throw new InvalidOperationException("Workflow validation failed.");
            }
            return this;
        }

        public MalleableWorkflowContext WorkflowContext { get; protected set; }

        public MalleableWorkflowCoordinatorFactory WithExternalStepActions()
        {
            foreach (var step in WorkflowContext.Workflow.Spec!.Steps!)
            {
                var executorType = step.Value.ExecutorType ?? "Akka.Net";
                if(executorType != "Akka.Net")
                {
                    var action = ExternalStepActionMapper.Create(WorkflowContext, step.Key);
                    if (action != null)
                    {
                        WorkflowContext.ExternalSteps.Add(step.Key, action);
                        WorkflowContext.Destinations.Add(step.Key, action.GetConnection());
                    }
                }
            }
            return this;
        }

        public IActorRef BuildCoordinatorActor()
        {
            var actorProps = Props.Create<MalleableWorkflowCoordinatorActor>(new object[] { WorkflowContext, GetActions(), GetValidators() });
            return System.ActorOf(actorProps, Workflow.Metadata.Name);
        }
    }
}