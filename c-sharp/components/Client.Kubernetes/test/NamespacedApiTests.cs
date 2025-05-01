using k8s;
using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes.Tests;
using k8s.Models;
using Kadense.Testing;
using Xunit.Abstractions;

namespace Kadense.Client.Kubernetes.Tests {
    public class NamespacedApiTests : KadenseTest
    {
        public NamespacedApiTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateAsync()
        {

            KubernetesClientFactory clientFactory = new KubernetesClientFactory();

            
            var server = new MockedKubeApiServer();
            server.OnListNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ListNamespacedObject.json");
                await ctx.Response.WriteAsync(value);
            };
            server.OnWatchNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => {
                var watchCreated = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchCreated.json");
                var watchModified = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchModified.json");
                var watchDeleted = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchDeleted.json");
                
                foreach (var watchResponse in new string[] { watchCreated, watchModified, watchDeleted} )
                {
                    var watchResponseAsString = watchResponse;
                    await ctx.Response.WriteAsync(watchResponseAsString);
                    await Task.Delay(100);
                }
            };

            server.OnReadNamespacedObjectAsync = (apiGroup, version, @namespace, kind, name) =>  Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadNamespacedObject.json"));

            server.OnPutNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadNamespacedObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            server.OnPostNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, ctx) => 
            {   
                var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadNamespacedObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            server.OnDeleteNamespacedObjectAsync = async (apiGroup, version, @namespace, kind, name, ctx) => {
                var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadNamespacedObject.json");
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(value);
                await ctx.Response.CompleteAsync();
            };

            server.Start(this.Output);
            var client = server.CreateClient();
        
            GenericClient genericClient = new GenericClient(client, "apps","v1","deployments");
            var list = await genericClient.ListNamespacedAsync<V1DeploymentList>("default");
            var item = list.Items.First();
            if (list.Items.Count() > 0)
            {
                // Delete the CRD
                await genericClient.DeleteNamespacedAsync<V1Deployment>(item.Metadata.NamespaceProperty, item.Metadata.Name);
            }

            var created = await genericClient.CreateNamespacedAsync<V1Deployment>(item, item.Metadata.NamespaceProperty);
            
            var updated = await genericClient.ReplaceNamespacedAsync<V1Deployment>(created, created.Metadata.NamespaceProperty, created.Metadata.Name);

            await genericClient.PatchNamespacedAsync<V1Deployment>(
                new V1Patch("{\"metadata\":{\"annotation\":{\"test\" : \"123\"}}}", V1Patch.PatchType.MergePatch),
                updated.Metadata.NamespaceProperty,
                updated.Metadata.Name
            );
        }
    }
}