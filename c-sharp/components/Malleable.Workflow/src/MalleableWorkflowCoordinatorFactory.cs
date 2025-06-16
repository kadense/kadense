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
        public MalleableWorkflowAction AddAction(string name, Type baseType)
        {
            ActionMapper = ActionMapper.AddNext(name, baseType);
            return ActionMapper;
        }
        public MalleableWorkflowAction AddAction(Type baseType)
        {
            ActionMapper = ActionMapper.AddNext(baseType);
            return ActionMapper;
        }

        public MalleableWorkflowApiAction AddApiAction(string name, Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> actionFunction)
        {
            ApiActionMapper = ApiActionMapper.AddNext(name, actionFunction);
            return ApiActionMapper;
        }

        public MalleableWorkflowExternalStepAction AddExternalStepAction(string name, MalleableWorkflowConnection connection)
        {
            Func<MalleableWorkflowContext, string, MalleableWorkflowStepWithExternalQueue> actionFunction = (context, stepName) =>
            {
                var @namespace = context.Workflow.Metadata.NamespaceProperty;
                var name = context.Workflow.Metadata.Name;
                var step = context.Workflow.Spec!.Steps![stepName];
                var inputType = context.StepInputTypes[stepName];
                var outputType = context.StepOutputTypes[stepName];
                var processorType = this.GetActions().Create(context, stepName)!;

                var type = typeof(MalleableWorkflowStepWithExternalQueue<,,,>).MakeGenericType(new Type[] { inputType, outputType, processorType, connection.GetType() });
                var stepWithExternalQueue = (MalleableWorkflowStepWithExternalQueue)Activator.CreateInstance(type, new object[] { context, stepName, connection })!;
                return stepWithExternalQueue;
            };
            ExternalStepActionMapper = ExternalStepActionMapper.AddNext(name, actionFunction);
            return ExternalStepActionMapper;
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

        public MalleableWorkflowCoordinatorFactory WithMessageSigning(bool enableMessageSigning = true)
        {
            this.WorkflowContext.EnableMessageSigning = enableMessageSigning;
            return this;
        }

        public MalleableWorkflowCoordinatorFactory WithWorkflowProvider(string providerName, MalleableWorkflowProvider provider)
        {
            if (WorkflowContext.Providers.ContainsKey(providerName))
            {
                WorkflowContext.Providers[providerName] = provider;
            }
            else
            {
                WorkflowContext.Providers.Add(providerName, provider);
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