using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes;
using Kadense.Client.Kubernetes;
using System.Reflection;
using k8s;
using Kadense.Models.Kubernetes.CoreApi;

namespace Kadense.Models.Jupyternetes.Tests {
    public class CustomResourceTests : KadenseTest
    {
        private JupyterNotebookTemplate CreateTemplate()
        {
            return new JupyterNotebookTemplate()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
                    Name = "test-template",
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookTemplateSpec()
                {
                    Pods = new List<NotebookTemplatePodSpec>(){
                        new NotebookTemplatePodSpec(){
                            Name = "test-pod",
                            Labels = new Dictionary<string, string>()
                            {
                                { "jupyternetes.kadense.io/testProperty" , "{test}-instance" }
                            },
                            Spec = new V1PodSpec()
                            {
                                Containers = new List<V1Container>()
                                {
                                    new V1Container() {
                                        Name = "test-container",
                                        Image = "who-knows" 
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        [TestOrder(0)]
        [Fact]
        public void SerializeTemplateTest()
        {
            var template = CreateTemplate();
            var json = System.Text.Json.JsonSerializer.Serialize<JupyterNotebookTemplate>(
                value: template, 
                options: new System.Text.Json.JsonSerializerOptions(){
                    WriteIndented = true,
                    MaxDepth = 0
                });
            Assert.NotEmpty(json);
        }

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

        

        

        [TestOrder(3)]
        [Fact]
        public async Task GenerateTemplateAsync()
        {
            var item = CreateTemplate();
            await CreateOrUpdateItem<JupyterNotebookTemplate>(item);
        }

        [TestOrder(4)]
        [Fact]
        public async Task GenerateInstanceAsync()
        {
            var variables = new Dictionary<string, string>();
            variables.Add("test", "test2");

            var item = new JupyterNotebookInstance()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
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

        [TestOrder(5)]
        [Fact]
        public async Task GeneratePodAsync()
        {
            var client = CreateClient();
            var crFactory = new CustomResourceClientFactory();
            var templateClient = crFactory.Create<JupyterNotebookTemplate>(client);
            var instanceClient = crFactory.Create<JupyterNotebookInstance>(client);
            var template = await templateClient.ReadNamespacedAsync("default", "test-template"); 
            var instance = await instanceClient.ReadNamespacedAsync("default", "test-instance");
            var pods = template.Spec!.CreatePods(instance);
            var newPod = pods.First();
            var existingPods = await client.CoreV1.ListNamespacedPodAsync("default"); 
            var filteredPods = existingPods.Items.Where(x => x.Metadata.Name == "test-pod");
            if(filteredPods.Count() > 0)
            {
                var existingPod = filteredPods.First();
                await client.CoreV1.DeleteNamespacedPodAsync(
                    namespaceParameter: "default", 
                    name: newPod.Metadata.Name
                    );
            }
            
            var createdPod = await client.CoreV1.CreateNamespacedPodAsync(newPod, "default");
            Assert.NotNull(createdPod);
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
            var existingItems = await genericClient.ListNamespacedAsync(item.Metadata.NamespaceProperty);
            if (existingItems.Items.Count > 0)
            {
                item.Metadata.ResourceVersion = existingItems.Items.First().Metadata.ResourceVersion;
                await genericClient.ReplaceNamespacedAsync(item);
            }
            else
            {
                var createdItem = await genericClient.CreateNamespacedAsync(item);
            } 
        }
        

        private async Task CreateCrdAsync<T>()
        {
            var client = CreateClient();

            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<T>();

            GenericClient genericClient = new GenericClient(client, "apiextensions.k8s.io","v1","customresourcedefinitions");
            var crds = await genericClient.ListAsync<k8s.Models.V1CustomResourceDefinitionList>();
            
            var items = crds.Items
                .Where(x => x.Metadata.Name == crd.Metadata.Name)
                .ToList();

            if (items.Count > 0)
            {
                // Delete the CRD
                crd.Metadata.ResourceVersion = items.First().Metadata.ResourceVersion;
                await genericClient.ReplaceAsync<k8s.Models.V1CustomResourceDefinition>(crd, crd.Metadata.Name);
            }
            else
            {
                var createdCrd = await genericClient.CreateAsync<k8s.Models.V1CustomResourceDefinition>(crd);
            }
        }
    }
}