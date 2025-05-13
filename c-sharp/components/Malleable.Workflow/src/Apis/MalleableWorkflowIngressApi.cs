using Kadense.Logging;
using Kadense.Malleable.API;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Kadense.Malleable.Workflow.Apis
{
    public class MalleableWorkflowIngressApi : MalleableApiBase
    {
        public MalleableWorkflowIngressApi(IList<MalleableAssembly> assemblies, MalleableWorkflowContext workflowContext, string apiName, string prefix = "/api/namespaces") : base(assemblies, prefix)
        {
            ApiName = apiName;
            WorkflowContext = workflowContext;
            StepName = WorkflowContext.Workflow.Spec!.APIs![apiName].IngressOptions!.NextStep!;
            Connection = workflowContext.Destinations[StepName];
        }

        private KadenseLogger<MalleableWorkflowIngressApi> logger = new KadenseLogger<MalleableWorkflowIngressApi>();

        public string ApiName { get; set; }
        public string StepName { get; set; }

        public MalleableWorkflowConnection Connection { get; set; }

        public MalleableWorkflowContext WorkflowContext { get; set; }
        
        private string processSignature = string.Empty;

        protected override async Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            var identifier = Guid.NewGuid().ToString();
            MalleableEnvelope<T> envelope = new MalleableEnvelope<T>(content, identifier, $"api://{ApiName}");
    
            if(WorkflowContext.EnableMessageSigning)
            {
                envelope.GenerateSignatures(null, processSignature);
                
                lock(processSignature)
                {
                    processSignature = envelope.ProcessSignature!;
                }
            }
            logger.LogInformation($"{envelope.Step} {StepName ?? "{abandoned}"} {envelope.LineageId} {envelope.RawSignature} {envelope.LineageSignature} {envelope.ProcessSignature} {envelope.CombinedSignature} ");
            Connection.Send(envelope);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            context.Response.Headers.Append("X-Identifier", identifier);
            await context.Response.WriteAsJsonAsync<T>(content, this.GetJsonSerializerOptions());
            await context.Response.CompleteAsync();
        }
    }

}