using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;
using Kadense.Client.Kubernetes;
using Xunit.Abstractions;
using Kadense.Client.Kubernetes.Tests;
using k8s;

namespace Kadense.Models.Jupyternetes.Tests {

    public class JupyternetesMockApi : MockedKubeApiServer
    {
        public string GetSuffix(string kind)
        {
            switch (kind)
            {
                case "jupyternotebookinstances":
                    return "Instance";
                case "jupyternotebooktemplates":
                    return "Template";
                case "pods":
                    return "Pod";
                case "persistentvolumeclaims":
                    return "Pvc";
                case "events":
                    return "Event";
                default:
                    throw new NotImplementedException($"Kind {kind} not implemented");
            }
        }

        public JupyternetesMockApi() : base()
        {
            OnListNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                    var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.List{GetSuffix(kind)}Object.json");
                    await ctx.Response.WriteAsync(value);
                };
            OnWatchNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                var watchCreated = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Watch{GetSuffix(kind)}Created.json");
                var watchModified = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Watch{GetSuffix(kind)}Modified.json");
                var watchDeleted = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Watch{GetSuffix(kind)}Deleted.json");
                
                foreach (var watchResponse in new string[] { watchCreated, watchModified, watchDeleted} )
                {
                    var watchResponseAsString = watchResponse;
                    await ctx.Response.WriteAsync(watchResponseAsString);
                    await Task.Delay(100);
                }
            };

            OnReadNamespacedObjectAsync = (apiGroup, version, @namespace, kind, name) =>  {
                return Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Read{GetSuffix(kind)}Object.json"));
            };

            OnPutNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Read{GetSuffix(kind)}Object.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            OnPostNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Read{GetSuffix(kind)}Object.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            OnDeleteNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => {
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Jupyternetes.Tests.Resources.Read{GetSuffix(kind)}Object.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };
        }
    }
}