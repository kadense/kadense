using Kadense.Client.Kubernetes.Tests;

namespace Kadense.Models.Malleable.Tests
{
    public class MalleableMockApi : MockedKubeApiServer
    {
        public MalleableMockApi() : base()
        {
            OnListNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                    var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.ListObject.json");
                    await ctx.Response.WriteAsync(value);
                };
            OnWatchNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                var watchCreated = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.WatchCreated.json");
                var watchModified = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.WatchModified.json");
                var watchDeleted = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.WatchDeleted.json");
                
                foreach (var watchResponse in new string[] { watchCreated, watchModified, watchDeleted} )
                {
                    var watchResponseAsString = watchResponse;
                    await ctx.Response.WriteAsync(watchResponseAsString);
                    await Task.Delay(100);
                }
            };

            OnReadNamespacedObjectAsync = (apiGroup, version, @namespace, kind, name) =>  {
                return Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.ReadObject.json"));
            };

            OnPutNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.ReadObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            OnPostNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.ReadObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            OnDeleteNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => {
                var value = KadenseTestUtils.GetEmbeddedResourceAsString($"Kadense.Models.Malleable.Tests.Resources.ReadObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };
        }
    }
}