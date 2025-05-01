using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes;
using Kadense.Client.Kubernetes;
using System.Reflection;
using k8s;
using Kadense.Models.Kubernetes.CoreApi;
using Xunit.Abstractions;

namespace Kadense.Models.Jupyternetes.Tests {
    public class CustomResourceTests : KadenseTest
    {
        public CustomResourceTests(ITestOutputHelper output) : base(output)
        {

        }
        public CustomResourceTestUtils TestUtils { get; set; } = new CustomResourceTestUtils();

        [TestOrder(0)]
        [Fact]
        public void SerializeTemplateTest()
        {
            var template = TestUtils.CreateTemplate();
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
            await TestUtils.CreateCrdAsync<JupyterNotebookTemplate>();
        }

        [TestOrder(2)]
        [Fact]
        public async Task GenerateCrdInstanceAsync()
        {
            await TestUtils.CreateCrdAsync<JupyterNotebookInstance>();
        }

        [TestOrder(3)]
        [Fact]
        public async Task GenerateTemplateAsync()
        {
            var item = TestUtils.CreateTemplate();
            await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(item);
        }

        [TestOrder(4)]
        [Fact]
        public async Task GenerateInstanceAsync()
        {   
            var item = TestUtils.CreateInstance();
            await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(item);
        }

        // [TestOrder(5)]
        // [Fact]
        // public async Task GeneratePodAsync()
        // {
        //     var client = TestUtils.CreateClient();
        //     var crFactory = new CustomResourceClientFactory();
        //     var templateClient = crFactory.Create<JupyterNotebookTemplate>(client);
        //     var instanceClient = crFactory.Create<JupyterNotebookInstance>(client);
        //     var template = await templateClient.ReadNamespacedAsync("default", "test-template"); 
        //     var instance = await instanceClient.ReadNamespacedAsync("default", "test-instance");
        //     var (pods, conversionIssues) = template.CreatePods(instance);
        //     var newPod = pods.First();
        //     var existingPods = await client.CoreV1.ListNamespacedPodAsync("default"); 
        //     var filteredPods = existingPods.Items.Where(x => x.Metadata.Name == newPod.Metadata.Name);
        //     if(filteredPods.Count() > 0)
        //     {
        //         var existingPod = filteredPods.First();
        //         await client.CoreV1.DeleteNamespacedPodAsync(
        //             namespaceParameter: "default", 
        //             name: newPod.Metadata.Name
        //             );

        //         Thread.Sleep(1000);
        //     }

        //     var createdPod = await client.CoreV1.CreateNamespacedPodAsync(newPod, "default");
        //     Assert.NotNull(createdPod);
        // }
    }
}