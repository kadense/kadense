using Kadense.Client.Kubernetes;
using Kadense.Logging;
using Xunit.Abstractions;

namespace Kadense.Models.Malleable.Tests {
    public class MalleableWorkflowTests : KadenseTest
    {
        public MalleableWorkflowTests(ITestOutputHelper output) : base(output)
        {

        }

        public MalleableMockers Mockers { get; set; } = new MalleableMockers();

        [Fact]
        public void TestWorkflowValidation()
        {
            var logger = new KadenseTestLogger(Output, LogLevel.Debug, "WorkflowValidation");
            var workflow = Mockers.MockWorkflow();
            Assert.True(workflow.Spec!.IsValid(logger));
        }
    }
}