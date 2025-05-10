using Kadense.Malleable.API;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Malleable.Workflow.Apis
{
    public class MalleableWorkflowIngressApi : MalleableApiBase
    {
        public MalleableWorkflowIngressApi(MalleableWorkflowContext workflowContext, string apiName, string prefix = "/api/namespaces") : base(prefix)
        {
            ApiName = apiName;
            WorkflowContext = workflowContext;
            StepName = WorkflowContext.Workflow.Spec!.APIs![apiName].IngressOptions!.NextStep!;
            Connection = workflowContext.Destinations[StepName];
        }

        public string ApiName { get; set; }
        public string StepName { get; set; }

        public MalleableWorkflowConnection Connection { get; set; }

        public MalleableWorkflowContext WorkflowContext { get; set; }
        

        protected override async Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            var identifier = Guid.NewGuid().ToString();
            
            Connection.Send<T>(content);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Headers.Append("X-Identifier", identifier);
            await context.Response.WriteAsJsonAsync<T>(content);
            await context.Response.CompleteAsync();
        }
    }

}