using Kadense.Logging;
using Kadense.Models.Jupyternetes;
using Kadense.Models.Kubernetes.CoreApi;
using Kadense.Models.Jupyternetes.Tests;
using Microsoft.Extensions.Logging;

namespace Kadense.Jupyternetes.Pods.Operator.Tests {

    public class PodTests : KadenseTest
    {
        public CustomResourceTestUtils TestUtils = new CustomResourceTestUtils();
    
        [TestOrder(0)]
        [Fact]
        public async Task CreateJupyternetesInstanceAndTemplate()
        {
            var instance = TestUtils.CreateInstance(instanceName: "jo-test-instance", templateName: "jo-test-template");
            await TestUtils.CreateOrUpdateItem<JupyterNotebookInstance>(instance);

            
            var template = TestUtils.CreateTemplate(templateName: "jo-test-template");
            await TestUtils.CreateOrUpdateItem<JupyterNotebookTemplate>(template);
        }

        [TestOrder(1)]
        [Fact]
        public async Task OnAddedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: "jo-test-instance", templateName: "jo-test-template");
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnAddedAsync(instance);
        }

        [TestOrder(2)]
        [Fact]
        public async Task OnUpdatedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: "jo-test-instance", templateName: "jo-test-template");
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnUpdatedAsync(instance);
        }

        [TestOrder(3)]
        [Fact]
        public async Task OnDeletedTest()
        {
            var instance = TestUtils.CreateInstance(instanceName: "jo-test-instance", templateName: "jo-test-template");
            KadenseLogger<PodTests> logger = new KadenseLogger<PodTests>();
            var watcherService = new PodWatcherService(logger);
            await watcherService.OnDeletedAsync(instance);
        }
    }
}