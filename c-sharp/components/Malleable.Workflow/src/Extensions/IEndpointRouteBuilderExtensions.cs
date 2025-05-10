using Kadense.Malleable.API;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Apis;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Malleable.Workflow.Extensions
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapWorkflow(this IEndpointRouteBuilder endpoints, MalleableWorkflowContext workflowContext, string prefix = "/api/namespaces")
        {
            var workflow = workflowContext.Workflow;
            foreach(var api in workflowContext.Workflow!.Spec!.APIs!)
            {
                switch(api.Value.ApiType)
                {
                    case "Ingress":
                        var apiPrefix = $"{prefix}/{workflow.Metadata.NamespaceProperty}/{workflow.Metadata.Name}";
                        var ingressApi = new MalleableWorkflowIngressApi(workflowContext, api.Key, prefix);
                        var underlyingTypeRef = workflowContext.Workflow.Spec!.APIs![api.Key].UnderlyingType;
                        var type = workflowContext.Assemblies[underlyingTypeRef!.GetQualifiedModuleName()].Types[underlyingTypeRef.ClassName!];
                        endpoints.MapMalleableApi(ingressApi, type, apiPrefix, api.Key);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown API type {api.Value.ApiType}");
                }
            }
            return endpoints;           
        }   
    }
}