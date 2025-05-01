using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;
using Kadense.Jupyternetes.Watchers;
using Xunit.Abstractions;
using Kadense.Client.Kubernetes;
using k8s;

namespace Kadense.Jupyternetes.Watchers.Tests {

    public class PvcTests : KadenseTest
    {
        public PvcTests(ITestOutputHelper output) : base(output)
        {
            Server.Start(output);
            this.K8sClient = Server.CreateClient();
        }

        public IKubernetes K8sClient { get; set; }
        public JupyternetesMockApi Server { get; set;} = new JupyternetesMockApi();
        public readonly string INSTANCE_NAME = "jo-pvc-test";
        public readonly string TEMPLATE_NAME = "jo-pvc-test";
        public CustomResourceTestUtils TestUtils = new CustomResourceTestUtils();
    
        // [TestOrder(0)]
        // [Fact]
        // public async Task CreateJupyternetesInstanceAndTemplate()
        // {
        //     var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
        //     await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(instance);

            
        //     var template = TestUtils.CreateTemplate(templateName: TEMPLATE_NAME);
        //     await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(template);
        // }

        [TestOrder(1)]
        [Fact]
        public async Task OnAddedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(K8sClient, logger);
            await watcherService.OnAddedAsync(instance);
        }

        [TestOrder(2)]
        [Fact]
        public async Task OnUpdatedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(K8sClient, logger);
            await watcherService.OnUpdatedAsync(instance);
        }

        [TestOrder(3)]
        [Fact]
        public async Task OnDeletedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(K8sClient, logger);
            await watcherService.OnDeletedAsync(instance);
        }
    }
}