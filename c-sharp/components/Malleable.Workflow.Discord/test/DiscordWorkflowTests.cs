using System.Text.Json;
using Kadense.Malleable.Workflow.Discord;
using Kadense.Malleable.Workflow.Discord.Models;
using Kadense.Malleable.Workflow.Tests;
using Kadense.Malleable.Workflow.Extensions;
using Akka.Actor;
using Kadense.Testing;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;
using Kadense.Models.Malleable;
using Kadense.Malleable.Reflection;
using System.Net.Http.Json;

namespace Kadense.Malleable.Workflow.Discord.Tests {
    public class DiscordWorkflowTests : KadenseTest
    {
        public DiscordWorkflowTests(ITestOutputHelper output) : base(output)
        {
            System = ActorSystem.Create($"kadense-{this.GetType().Name}");
        }

        ActorSystem System { get; set; }

        [Fact]
        public async Task WorkflowTests()
        {
            var mocker = new InternalMocker();
            var assemblyFactory = mocker
                .GetAssemblyFactory()
                .AddDiscord();


            this.Assemblies = assemblyFactory.GetAssemblies().Values.ToList();

            var workflow = mocker
                .GetWorkflowFactory()
                .AddDiscordApi()
                    .AddStep("DiscordCommand", "DiscordCommand")
                        .SetOutputType("malleable", "discord", "DiscordRequestAndResponse")
                        .AddNextStep("WriteApi2", "WriteApi")
                            .SetParameter("baseUrl", "http://localhost:8080")
                            .SetParameter("Path", "\"test\"")
                        .EndStep() // End WriteApi step
                    .EndStep() // End DiscordCommand step
                .EndApi()
                .EndWorkflow();

            var server = new MalleableWorkflowApiMockServer();
            var builder = System
                .AddWorkflow(workflow, assemblyFactory)
                .AddDiscord(new TestDiscordCommandProvider())
                .WithDebugMode()
                .WithExternalStepActions();

            var results = new List<MalleableBase>();
            var actorRef = builder.BuildCoordinatorActor();
            server.Start(Output, builder.WorkflowContext, builder.GetApiActions(), async (HttpContext ctx) =>
            {
                var result = await ctx.Request.ReadFromJsonAsync(typeof(DiscordRequestAndResponse));
                results.Add((MalleableBase)result!);
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 200;
                await ctx.Response.WriteAsJsonAsync(result, this.GetJsonSerializerOptions());
                await ctx.Response.CompleteAsync();
            });

            var url = server.GetUrl();
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            builder.WorkflowContext.Workflow.Spec!.Steps!["WriteApi2"].Options!.Parameters["baseUrl"] = url;

            string json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.Discord.Tests.Resources.slash-command-example.json");
            var instance = JsonSerializer.Deserialize<DiscordInteraction>(json)!;

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
            {
                RequestUri = new Uri($"{url}/api/namespaces/test-namespace/test-workflow/Discord"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(instance, instance.GetType(), options: GetJsonSerializerOptions())
            });

            response.EnsureSuccessStatusCode();
            await Task.Delay(1000);

            Assert.NotEmpty(results);
            Assert.Single(results);
            var convertedInstance = results.First();
            Assert.IsType<DiscordRequestAndResponse>(convertedInstance);
            var discordResponse = (DiscordRequestAndResponse)convertedInstance;
            Assert.NotNull(discordResponse.Request);
            Assert.NotNull(discordResponse.Response);
            Assert.Equal("Test for 'The Gitrog Monster'", discordResponse.Response.Data!.Content);
        }

        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }
        protected IList<MalleableAssembly>? Assemblies { get; set; }
        
        
        protected JsonSerializerOptions GetJsonSerializerOptions()
        {
            if (TypeResolver == null)
            {
                TypeResolver = new MalleablePolymorphicTypeResolver();
                foreach (var assembly in Assemblies!)
                {
                    TypeResolver.MalleableAssembly.Add(assembly);
                }
            }
            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = TypeResolver,
                WriteIndented = true
            };

            return options;
        }
    }
}