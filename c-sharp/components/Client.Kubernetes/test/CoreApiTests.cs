using k8s;
using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes.Tests;
using k8s.Models;
using Kadense.Testing;
using Xunit.Abstractions;

namespace Kadense.Client.Kubernetes.Tests {
    public class CoreApiTests : KadenseTest
    {
        public CoreApiTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateAsync()
        {

            KubernetesClientFactory clientFactory = new KubernetesClientFactory();

            IKubernetes? client = null;
            
            var server = new MockedKubeApiServer();
            server.Start(this.Output);
            client = server.CreateClient();
        
            GenericClient genericClient = new GenericClient(client, "","v1","pods");
            var list = await genericClient.ListNamespacedAsync<V1PodList>("default");
            var item = list.Items.First();
            if (list.Items.Count() > 0)
            {
                // Delete the CRD
                await genericClient.DeleteNamespacedAsync<V1Pod>(item.Metadata.NamespaceProperty, item.Metadata.Name);
            }

            var created = await genericClient.CreateNamespacedAsync<V1Pod>(item, item.Metadata.NamespaceProperty);
            
            var updated = await genericClient.ReplaceNamespacedAsync<V1Pod>(created, created.Metadata.NamespaceProperty, created.Metadata.Name);

            await genericClient.PatchNamespacedAsync<V1Pod>(
                new V1Patch("{\"metadata\":{\"annotation\":{\"test\" : \"123\"}}}", V1Patch.PatchType.MergePatch),
                updated.Metadata.NamespaceProperty,
                updated.Metadata.Name
            );
        }
    }
}