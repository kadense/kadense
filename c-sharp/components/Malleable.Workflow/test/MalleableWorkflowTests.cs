using System.Net.Http.Json;
using System.Text.Json;
using Akka.Actor;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Reflection.Tests;
using Kadense.Malleable.Workflow.Extensions;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Models.Malleable.Tests;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace Kadense.Malleable.Workflow.Tests {
    public class MalleableWorkflowTests : KadenseTest
    {
        public MalleableWorkflowTests(ITestOutputHelper output) : base(output)
        {
            System = ActorSystem.Create($"kadense-{this.GetType().Name}");
        }

        ActorSystem System { get; set; }

        [Fact]
        public async Task TestAkkaWorkflowAsync()
        {
            var mocker = new MalleableMockers();
            var moduleDefinition = mocker.MockModule();
            var converterDefinition = mocker.MockConverterModule();
            var malleableAssemblyFactory = new MalleableAssemblyFactory();
            var malleableAssembly = malleableAssemblyFactory.WithNewAssembly(moduleDefinition);
            var converterAssembly = malleableAssemblyFactory.WithNewAssembly(converterDefinition);
            Assemblies = malleableAssemblyFactory.GetAssemblies().Values.ToList();
            var workflow = mocker.MockWorkflow();
            
            var results = new List<MalleableBase>();
            var builder = System
            .AddWorkflow(workflow, malleableAssemblyFactory)
            .WithDebugMode()
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
            await Task.Delay(1000);

            Assert.NotEmpty(results);
            Assert.Single(results);
            var convertedInstance = results.First();
            Assert.IsType(malleableAssembly.Types["ConvertedClass"]!, convertedInstance);
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