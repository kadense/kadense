using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;
using Kadense.Jupyternetes.Watchers;

namespace Kadense.Jupyternetes.Watchers.Tests {

    public class PvcTests : KadenseTest
    {
        public readonly string INSTANCE_NAME = "jo-pvc-test";
        public readonly string TEMPLATE_NAME = "jo-pvc-test";
        public CustomResourceTestUtils TestUtils = new CustomResourceTestUtils();
    
        [TestOrder(0)]
        [Fact]
        public async Task CreateJupyternetesInstanceAndTemplate()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(instance);

            
            var template = TestUtils.CreateTemplate(templateName: TEMPLATE_NAME);
            await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(template);
        }

        [TestOrder(1)]
        [Fact]
        public async Task OnAddedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(logger);
            await watcherService.OnAddedAsync(instance);
        }

        [TestOrder(2)]
        [Fact]
        public async Task OnUpdatedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(logger);
            await watcherService.OnUpdatedAsync(instance);
        }

        [TestOrder(3)]
        [Fact]
        public async Task OnDeletedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: INSTANCE_NAME, templateName: TEMPLATE_NAME);
            KadenseLogger<PvcTests> logger = new KadenseLogger<PvcTests>();
            var watcherService = new JupyternetesPvcWatcherService(logger);
            await watcherService.OnDeletedAsync(instance);
        }
    }
}