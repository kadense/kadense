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
            await CreateCrdAsync<JupyterNotebookTemplate>();
        }

        [TestOrder(2)]
        [Fact]
        public async Task GenerateCrdInstanceAsync()
        {
            await CreateCrdAsync<JupyterNotebookInstance>();
        }

        

        [TestOrder(4)]
        [Fact]
        public async Task GenerateInstanceAsync()
        {
            var variables = new Dictionary<string, string>();
            variables.Add("test", "test2");

            var item = new JupyterNotebookInstance()
            {
                Metadata = new V1ObjectMeta(){
                    Name = "test-instance",
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookInstanceSpec()
                {
                    Template = new NotebookTemplate(){
                        Name = "test-template"
                    },
                    Variables = variables
                }
            };
            await CreateOrUpdateItem<JupyterNotebookInstance>(item);
        }
        
        private IKubernetes CreateClient()
        {
            KubernetesClientFactory clientFactory = new KubernetesClientFactory();
            return clientFactory.CreateClient();
        }

        private async Task CreateOrUpdateItem<T>(T item)
            where T : KadenseCustomResource
        {
            var client = CreateClient();
            var crFactory = new CustomResourceClientFactory();
            var genericClient = crFactory.Create<T>(client);
            var existingItems = await genericClient.ListNamespacedAsync<KadenseCustomResourceList<T>>(item.Metadata.NamespaceProperty);
            if (existingItems.Items.Count > 0)
            {
                // Delete the CRD
                item.Metadata.ResourceVersion = existingItems.Items.First().Metadata.ResourceVersion;
                await genericClient.ReplaceNamespacedAsync<T>(item, item.Metadata.NamespaceProperty, item.Metadata.Name);
            }
            else
            {
                var createdItem = await genericClient.CreateNamespacedAsync<T>(item, item.Metadata.NamespaceProperty);
            } 
        }
        

        private async Task CreateCrdAsync<T>()
        {
            var client = CreateClient();

            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<T>();

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