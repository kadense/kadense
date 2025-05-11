using Kadense.Malleable.Workflow.Apis;
using Kadense.Malleable.API;
using Microsoft.AspNetCore.Routing;
using Kadense.Malleable.Workflow.Extensions;

namespace Kadense.Malleable.Workflow.Apis
{
    public class MalleableWorkflowApiAction 
    {
        public static MalleableWorkflowApiAction CreateDefault(MalleableWorkflowCoordinatorActorFactory factory)
        {
            return new MalleableWorkflowApiAction(factory, "Ingress", (context, apiName, endpoints) => {
                var workflow = context.Workflow;
                var api = workflow.Spec!.APIs![apiName];
                if(!api.IngressOptions!.Parameters.TryGetValue("prefix", out var prefix))
                    prefix = "/api/namespaces";

                var apiPrefix = $"{prefix}/{workflow.Metadata.NamespaceProperty}/{workflow.Metadata.Name}";
                var ingressApi = new MalleableWorkflowIngressApi(context, apiName, prefix);
                var underlyingTypeRef = api.UnderlyingType;
                var type = context.Assemblies[underlyingTypeRef!.GetQualifiedModuleName()].Types[underlyingTypeRef.ClassName!];
                return endpoints.MapMalleableApi(ingressApi, type, apiPrefix, apiName);
            });
        }

        public MalleableWorkflowApiAction(MalleableWorkflowCoordinatorActorFactory factory, string apiType, Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> actionFunction)
        {
            ApiType = apiType;
            ActionFunction = actionFunction;
            Factory = factory;
        }
        public MalleableWorkflowApiAction(MalleableWorkflowCoordinatorActorFactory factory, string apiType, Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> actionFunction, MalleableWorkflowApiAction previous) : this(factory, apiType, actionFunction)
        {
            Previous = previous;
        }

        private MalleableWorkflowCoordinatorActorFactory Factory { get; }

        private string ApiType { get; }

        private Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> ActionFunction { get; }

        private MalleableWorkflowApiAction? Previous { get; set; }
        private MalleableWorkflowApiAction? Next { get; set; }

        public MalleableWorkflowApiAction AddNext(string apiType, Func<MalleableWorkflowContext, string, IEndpointRouteBuilder, IEndpointRouteBuilder> actionFunction)
        {
            var next = new MalleableWorkflowApiAction(Factory, apiType, actionFunction, this);
            Next = next;
            return next;
        }

        public MalleableWorkflowCoordinatorActorFactory CompleteActions()
        {
            return Factory;
        }

        public IEndpointRouteBuilder Create(MalleableWorkflowContext context, string apiName, IEndpointRouteBuilder endpoints)
        {
            if(context.Workflow.Spec!.APIs![apiName].ApiType == ApiType)
            {
                return ActionFunction(context, apiName, endpoints);
            }
            else if(Next != null)
            {
                return Next.Create(context, apiName, endpoints);
            }

            throw new InvalidOperationException($"No action found for API {apiName}.");
        }

        public MalleableWorkflowApiAction GetFirst()
        {
            if (Previous != null)
            {
                return Previous.GetFirst();
            }
            return this;
        }
    }
}