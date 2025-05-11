using Kadense.Malleable.API;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Apis;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Malleable.Workflow.Extensions
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapWorkflow(this IEndpointRouteBuilder endpoints, MalleableWorkflowContext workflowContext, MalleableWorkflowApiAction action)
        {
            var workflow = workflowContext.Workflow;
            foreach(var api in workflow.Spec!.APIs!)
            {
                action.Create(workflowContext, api.Key, endpoints);
            }
            return endpoints;
        }
    }
}