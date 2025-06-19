using System.Text.Json;
using Kadense.Malleable.Workflow.Discord;
using Kadense.Malleable.Workflow.Discord.Models;
using Kadense.Malleable.Workflow.Tests;
using Kadense.Malleable.Workflow.Extensions;
using Akka.Actor;
using Kadense.Testing;
using Xunit.Abstractions;

namespace Kadense.Malleable.Workflow.Discord.Tests {
    public class DiscordWorkflowTests : KadenseTest
    {
        public DiscordWorkflowTests(ITestOutputHelper output) : base(output)
        {
            System = ActorSystem.Create($"kadense-{this.GetType().Name}");
        }

        ActorSystem System { get; set; }

        [Fact]
        public void WorkflowTests()
        {
            var mocker = new InternalMocker();
            var assemblyFactory = mocker
                .GetAssemblyFactory()
                .AddDiscord();

            var workflow = mocker
                .GetWorkflowFactory()
                .AddDiscordApi()
                    .AddStep("WriteApi2", "WriteApi")
                            .SetParameter("baseUrl", "http://localhost:8080")
                            .SetParameter("Path", "\"test\"")
                        .EndStep() // End WriteApi step
                .EndApi()
                .EndWorkflow();

            var server = new MalleableWorkflowApiMockServer();
            var builder = System
                .AddWorkflow(workflow, assemblyFactory)
                .AddDiscord()
                .WithDebugMode()
                .WithExternalStepActions();
               
        }
    }
}