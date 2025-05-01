using k8s;
using Kadense.Models.Kubernetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Client.Kubernetes;
using Kadense.Client.Kubernetes.Tests;

namespace Kadense.Models.Jupyternetes.Tests {
    public class CustomResourceTestUtils
    {
        public IKubernetes CreateClient()
        {
            var MockedKubeApiServer = new JupyternetesMockApi();
            MockedKubeApiServer.Start();
            return MockedKubeApiServer.CreateClient();
        }

        public async Task CreateOrUpdateItem<T>(T item)
            where T : KadenseCustomResource
        {
            var client = CreateClient();
            var crFactory = new CustomResourceClientFactory();
            var genericClient = crFactory.Create<T>(client);
            var existingItems = await genericClient.ListNamespacedAsync(item.Metadata.NamespaceProperty);
            var filteredItems = existingItems.Items
                .Where(x => x.Metadata.Name == item.Metadata.Name)
                .ToList();
            if (filteredItems.Count > 0)
            {
                item.Metadata.ResourceVersion = filteredItems.First().Metadata.ResourceVersion;
                await genericClient.ReplaceNamespacedAsync(item);
            }
            else
            {
                var createdItem = await genericClient.CreateNamespacedAsync(item);
            } 
        }
        

        public async Task CreateCrdAsync<T>()
        {
            var client = CreateClient();

            CustomResourceDefinitionFactory crdFactory = new CustomResourceDefinitionFactory();
            var crd = crdFactory.Create<T>();

            //GenericClient genericClient = new GenericClient(client, "apiextensions.k8s.io","v1","customresourcedefinitions");
            //var crds = await genericClient.ListAsync<k8s.Models.V1CustomResourceDefinitionList>();
            var crds = await client.ApiextensionsV1.ListCustomResourceDefinitionAsync();
            var items = crds.Items
                .Where(x => x.Metadata.Name == crd.Metadata.Name)
                .ToList();

            if (items.Count > 0)
            {
                // Delete the CRD
                crd.Metadata.ResourceVersion = items.First().Metadata.ResourceVersion;
                //await genericClient.ReplaceAsync<k8s.Models.V1CustomResourceDefinition>(crd, crd.Metadata.Name);
                await client.ApiextensionsV1.ReplaceCustomResourceDefinitionAsync(crd, crd.Metadata.Name);
            }
            else
            {
                // var createdCrd = await genericClient.CreateAsync<k8s.Models.V1CustomResourceDefinition>(crd);
                await client.ApiextensionsV1.CreateCustomResourceDefinitionAsync(crd);
            }
        }

        public virtual JupyterNotebookInstance CreateInstance(string instanceName = "test-instance", string templateName = "test-template")
        {
            var variables = new Dictionary<string, string>();
            variables.Add("test", "test2");

            return new JupyterNotebookInstance()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
                    Name = instanceName,
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookInstanceSpec()
                {
                    Template = new NotebookTemplate(){
                        Name = templateName
                    },
                    Variables = variables
                }
            };
        }

        public virtual JupyterNotebookTemplate CreateTemplate(string templateName = "test-template", string prefixName = "test", bool withPvcs = false)
        {
            return new JupyterNotebookTemplate()
            {
                Metadata = new k8s.Models.V1ObjectMeta(){
                    Name = templateName,
                    NamespaceProperty = "default"
                },
                Spec = new JupyterNotebookTemplateSpec()
                {
                    Pods = new List<NotebookTemplatePodSpec>(){
                        new NotebookTemplatePodSpec(){
                            Name = $"{prefixName}-pod",
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
                                        Image = "who-knows",
                                        VolumeMounts = withPvcs ? new List<V1VolumeMount>()
                                        {
                                            new V1VolumeMount()
                                            {
                                                Name = "test-pvc",
                                                MountPath = "/mnt/test"
                                            }
                                        } : null
                                    },
                                },
                                Volumes = withPvcs ? new List<V1Volume>()
                                {
                                    new V1Volume()
                                    {
                                        Name = "test-pvc",
                                        PersistentVolumeClaim = new V1PersistentVolumeClaimVolumeSource()
                                        {
                                            ClaimName = $"{{jupyternetes.pvcs.{prefixName}-pvc}}"
                                        }
                                    }
                                } : null
                            },
                        }
                    },
                    Pvcs = new List<NotebookTemplatePvcSpec>(){
                        new NotebookTemplatePvcSpec(){
                            Name = $"{prefixName}-pvc",
                            Labels = new Dictionary<string, string>()
                            {
                                { "jupyternetes.kadense.io/testProperty" , "{test}-instance" }
                            },
                            Spec = new V1PersistentVolumeClaimSpec()
                            {
                                AccessModes = new List<string>() { "ReadWriteOnce" },
                                Resources = new V1VolumeResourceRequirements()
                                {
                                    Requests = new Dictionary<string, string>()
                                    {
                                        { "storage", "1Gi" }
                                    }
                                }
                            },
                        }
                    }
                }
            };
        }

    }
}