using Akka.Actor;
using Xunit.Abstractions;
using Kadense.Models.Malleable.Tests;
using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable;
using RabbitMQ.Client;
using Kadense.Malleable.Workflow.Extensions;
using Kadense.Malleable.Workflow.Tests;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Kadense.Malleable.Workflow.Queuing.RabbitMQ.Tests {
    public class MalleableWorkflowRMQTests : KadenseTest
    {
        public MalleableWorkflowRMQTests(ITestOutputHelper output) : base(output)
        {
            System = ActorSystem.Create($"kadense-{this.GetType().Name}");
        }

        ActorSystem System { get; set; }

        //[Fact]
        [Fact(Skip = "Requires RabbitMQ to be running")]
        [Trait("Category", "Integration")]
        public async Task TestAkkaWorkflowAsync()
        {
            var mocker = new MalleableMockers();
            var moduleDefinition = mocker.MockModule();
            var converterDefinition = mocker.MockConverterModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.WithNewAssembly(moduleDefinition);
            var converterAssembly = malleableAssemblyFactory.WithNewAssembly(converterDefinition);
            var workflow = mocker.MockWorkflow();
            workflow.Spec!.Steps!["TestStep"].ExecutorType = "RabbitMQ";
            workflow.Spec.Steps["WriteApi"].Options!.NextStep = "RMQWrite";
            workflow.Spec.Steps["RMQWrite"] = new MalleableWorkflowStep()
            {
                Action = "RabbitMQWriter",
                Options = new MalleableWorkflowStandardActionOptions()
                {
                    Parameters = new Dictionary<string, string>()
                    {
                    }
                }
            };
            var rabbitMqFactory = new ConnectionFactory{ HostName = "localhost", Port = 5672 };
            var rabbitMqConnection = await rabbitMqFactory.CreateConnectionAsync(); 
            var results = new List<MalleableBase>();
            var builder = System
            .AddWorkflow(workflow, malleableAssemblyFactory.GetAssemblies())
            .AddEnqueueProvider(new RabbitMQEnqueueProviderOptions())
            .AddEnqueueAction()
            .WithDebugMode()
            .Validate()
            .AddRabbitMQConnection(new RabbitMQConnectionOptions())
            .WithExternalStepActions();
            var server = new MalleableWorkflowApiMockServer();
            var actorRef = builder.BuildCoordinatorActor();
            server.Start(Output, builder.WorkflowContext, builder.GetApiActions(), async (HttpContext ctx) => {
                var result = await ctx.Request.ReadFromJsonAsync(malleableAssembly.Types["ConvertedClass"]!);
                results.Add((MalleableBase)result!);
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 200;
                await ctx.Response.WriteAsJsonAsync(result, this.GetJsonSerializerOptions());
                await ctx.Response.CompleteAsync();
            });
            var url = server.GetUrl();
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            builder.WorkflowContext.Workflow.Spec!.Steps!["WriteApi"].Options!.Parameters["baseUrl"] = url;
            
            var instance = (MalleableBase)Activator.CreateInstance(malleableAssembly.Types["TestInheritedClass"])!;
            instance.GetType().GetProperty("TestString")!.SetValue(instance, "test1");


            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
            {
                RequestUri = new Uri($"{url}/api/namespaces/test-namespace/test-workflow/TestApi"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(instance, instance.GetType(), options: GetJsonSerializerOptions())
            });

            response.EnsureSuccessStatusCode();
            await Task.Delay(2000);

            Assert.NotEmpty(results);
            Assert.Single(results);
            var convertedInstance = results.First();
            Assert.IsType(malleableAssembly.Types["ConvertedClass"]!, convertedInstance);
            Assert.Contains("TestStep", builder.WorkflowContext.ExternalSteps);
            Assert.Contains("TestStep", builder.WorkflowContext.Destinations);
            Assert.IsAssignableFrom<RabbitMQConnection>(builder.WorkflowContext.Destinations["TestStep"]);
            //var convertedInstance = await actor.Ask(instance);
            //Assert.NotNull(convertedInstance);
            //Assert.IsType(malleableAssembly.Types["ConvertedClass"]!, convertedInstance);
        }
        
        protected MalleablePolymorphicTypeResolver? TypeResolver { get; set; }
        protected IList<MalleableAssembly>? Assemblies { get; set; }
        protected JsonSerializerOptions GetJsonSerializerOptions()
        {
            if(TypeResolver == null)
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