using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;
using Kadense.Jupyternetes.Watchers;
using Kadense.Client.Kubernetes;

namespace Kadense.Jupyternetes.Watchers.Tests {

    public class CombinedTests : KadenseTest
    {
        public readonly string INSTANCE_NAME = "jo-combined-test";
        public readonly string TEMPLATE_NAME = "jo-combined-test";
        public CustomResourceTestUtils TestUtils = new CustomResourceTestUtils();

        public JupyterNotebookInstance? Instance = null;
    
        [TestOrder(0)]
        [Fact]
        public async Task CreateJupyternetesInstanceAndTemplate()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(Instance);

            
            var template = TestUtils.CreateTemplate(templateName: TEMPLATE_NAME, prefixName: "combined", withPvcs: true);
            await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(template);
        }

        [TestOrder(1)]
        [Fact]
        public async Task OnAddedTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnAddedAsync(Instance);
            Assert.Equal("Pending", Instance.Status.PodsProvisioningState);
            Assert.Equal("Pending", Instance.Status.PvcsProvisionedState);
        }

        [TestOrder(2)]
        [Fact]
        public async Task OnUpdatedPodTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Pending", Instance.Status.PodsProvisioningState);
            Assert.Equal("Pending", Instance.Status.PvcsProvisionedState);
        }

        

        [TestOrder(3)]
        [Fact]
        public async Task OnUpdatedPvcTest()
        {
            var client = new CustomResourceClientFactory().Create<JupyterNotebookInstance>(TestUtils.CreateClient());
            Instance = await client.ReadNamespacedAsync("default", INSTANCE_NAME);
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PvcWatcherService(logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
        }

        

        [TestOrder(4)]
        [Fact]
        public async Task OnUpdatedPodTest2()
        {
            var client = new CustomResourceClientFactory().Create<JupyterNotebookInstance>(TestUtils.CreateClient());
            Instance = await client.ReadNamespacedAsync("default", INSTANCE_NAME);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Completed", Instance.Status.PodsProvisioningState);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
            Assert.Single(Instance.Status.Pods);
            foreach(var pod in Instance.Status.Pods)
            {
                Assert.Equal("Processed", pod.State);
            }
        }

        [TestOrder(int.MaxValue)]
        [Fact]
        public async Task OnDeletedTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnDeletedAsync(Instance);
        }
    }
}