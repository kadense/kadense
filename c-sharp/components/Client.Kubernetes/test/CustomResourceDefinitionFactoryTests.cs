using k8s;
using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes.Tests;
using k8s.Models;

namespace Kadense.Client.Kubernetes.Tests {
    public class CustomResourceDefinitionFactoryTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<TestKubernetesObject>();

            KubernetesClientFactory clientFactory = new KubernetesClientFactory();
            var client = clientFactory.CreateClient();
        
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
        }
    }
}