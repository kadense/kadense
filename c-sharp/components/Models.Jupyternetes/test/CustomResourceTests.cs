using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes;
using Kadense.Client.Kubernetes;
using System.Reflection;
using k8s;
using k8s.Models;

namespace Kadense.Models.Jupyternetes.Tests {
    public class CustomResourceTests : KadenseTest
    {
        [TestOrder(1)]
        [Fact]
        public async Task GenerateCrdTemplateAsync()
        {
            await CreateAsync<JupyterNotebookTemplate>();
        }

        [TestOrder(2)]
        [Fact]
        public async Task GenerateCrdInstanceAsync()
        {
            await CreateAsync<JupyterNotebookInstance>();
        }
        
        private async Task CreateAsync<T>()
        {
            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<T>();

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
                crd.Metadata.ResourceVersion = items.First().Metadata.ResourceVersion;
                await genericClient.ReplaceAsync<V1CustomResourceDefinition>(crd, crd.Metadata.Name);
            }
            else
            {
                var createdCrd = await genericClient.CreateAsync<V1CustomResourceDefinition>(crd);
            }
        }
    }
}