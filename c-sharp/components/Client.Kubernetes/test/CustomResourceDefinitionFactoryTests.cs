using k8s;
using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes.Tests;
using k8s.Models;
using Kadense.Testing;
using Xunit.Abstractions;

namespace Kadense.Client.Kubernetes.Tests {
    public class CustomResourceDefinitionFactoryTests : KadenseTest
    {
        public CustomResourceDefinitionFactoryTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateAsync()
        {

            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<TestKubernetesObject>();

            KubernetesClientFactory clientFactory = new KubernetesClientFactory();

            
            var server = new MockedKubeApiServer();
            server.Start(this.Output);
            var client = server.CreateClient();
        
            GenericClient genericClient = new GenericClient(client, "apiextensions.k8s.io","v1","customresourcedefinitions");
            var crds = await genericClient.ListAsync<V1CustomResourceDefinitionList>();
            
            var items = crds.Items
                .Where(x => x.Metadata.Name == crd.Metadata.Name)
                .ToList();

            if (items.Count > 0)
            {
                // Delete the CRD
                await genericClient.DeleteAsync<V1CustomResourceDefinition>(crd.Metadata.Name);
            }

            var createdCrd = await genericClient.CreateAsync<V1CustomResourceDefinition>(crd);

            await genericClient.PatchAsync<V1CustomResourceDefinition>(
                new V1Patch("{\"metadata\":{\"annotation\":{\"test\" : \"123\"}}}", V1Patch.PatchType.MergePatch),
                createdCrd.Metadata.Name
            );
        }
    }
}