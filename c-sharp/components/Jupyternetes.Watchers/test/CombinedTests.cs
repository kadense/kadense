using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;
using Kadense.Jupyternetes.Watchers;
using Kadense.Client.Kubernetes;
using Xunit.Abstractions;
using Kadense.Client.Kubernetes.Tests;
using k8s;

namespace Kadense.Jupyternetes.Watchers.Tests {

    public class CombinedTests : KadenseTest
    {

        public CombinedTests(ITestOutputHelper output) : base(output)
        {
            
            TestUtils = new CustomResourceTestUtils();
            Server = new JupyternetesMockApi();
            Server.Start(output);
            this.K8sClient = Server.CreateClient();
        }

        public readonly string INSTANCE_NAME = "jo-combined-test";

        public readonly string TEMPLATE_NAME = "jo-combined-test";

        public CustomResourceTestUtils TestUtils = new CustomResourceTestUtils();

        public IKubernetes K8sClient { get; set; }
        public JupyternetesMockApi Server { get; set;}

        public JupyterNotebookInstance? Instance = null;
    
        // [TestOrder(0)]
        // [Fact]
        // public async Task CreateJupyternetesInstanceAndTemplate()
        // {
        //     Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
        //     await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(Instance);

            
        //     var template = TestUtils.CreateTemplate(templateName: TEMPLATE_NAME, prefixName: "combined", withPvcs: true);
        //     await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(template);
        // }

        [TestOrder(1)]
        [Fact]
        public async Task OnAddedTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new JupyternetesPodWatcherService(K8sClient, logger);
            await watcherService.OnAddedAsync(Instance);
            Assert.Equal("Pending", Instance.Status.PodsProvisionedState);
            Assert.Equal("Pending", Instance.Status.PvcsProvisionedState);
        }

        [TestOrder(2)]
        [Fact]
        public async Task OnUpdatedPodTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new JupyternetesPodWatcherService(K8sClient, logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Pending", Instance.Status.PodsProvisionedState);
            Assert.Equal("Pending", Instance.Status.PvcsProvisionedState);
        }

        [TestOrder(3)]
        [Fact]
        public async Task OnUpdatedPvcTest()
        {
            var client = new CustomResourceClientFactory().Create<JupyterNotebookInstance>(K8sClient);
            Instance = await client.ReadNamespacedAsync("default", INSTANCE_NAME);
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new JupyternetesPvcWatcherService(K8sClient, logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
        }

        

        [TestOrder(4)]
        [Fact]
        public async Task OnUpdatedPodTest2()
        {
            var client = new CustomResourceClientFactory().Create<JupyterNotebookInstance>(K8sClient);
            Instance = await client.ReadNamespacedAsync("default", INSTANCE_NAME);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new JupyternetesPodWatcherService(K8sClient, logger);
            await watcherService.OnUpdatedAsync(Instance);
            Assert.Equal("Completed", Instance.Status.PodsProvisionedState);
            Assert.Equal("Completed", Instance.Status.PvcsProvisionedState);
            Assert.Single(Instance.Status.Pods);
        }

        [TestOrder(int.MaxValue)]
        [Fact]
        public async Task OnDeletedTest()
        {
            Instance = Instance == null ? TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME) : Instance;
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new JupyternetesPodWatcherService(K8sClient, logger);
            await watcherService.OnDeletedAsync(Instance);
        }
    }
}