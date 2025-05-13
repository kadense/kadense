using System.Net.Http.Json;
using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable.Tests;
using Xunit.Abstractions;

namespace Kadense.Malleable.API.Tests
{
    public class MalleableIngressFileServerTests : KadenseTest
    {
        public MalleableIngressFileServerTests(ITestOutputHelper output) : base(output)
        {
            var server = new MalleableMockIngressFileServer();
            Client = new HttpClient();
            var mocker = new MalleableMockers();
            var module = mocker.MockModule();
            var assemblyBuilder = new MalleableAssemblyFactory();
            var assembly = assemblyBuilder.CreateAssembly(module);
            var assemblies = new List<MalleableAssembly> { assembly };
            InheritingType = assembly.Types["TestInheritedClass"];
            server.Start(assemblies, Output, [ InheritingType ]);

            Url = server.GetUrl();
            Client.BaseAddress = new Uri(Url);
            FileServer = new MalleableRestApiFileServer(assemblies);
        }

        public HttpClient Client { get; set; }

        public string Url { get; set; } 

        public MalleableRestApiFileServer FileServer { get; set; }

        public Type InheritingType { get; set; }

        [Fact]
        [TestOrder(1)]
        public async Task TestAllAsync()
        {
            
            var inheritedInstance = (IMalleableIdentifiable)Activator.CreateInstance(InheritingType)!;
            Assert.NotNull(inheritedInstance);

            InheritingType.GetProperty("TestString")!.SetValue(inheritedInstance, "test1");
            InheritingType.GetProperty("TestList")!.SetValue(inheritedInstance, new List<string> { "test2", "test3" });


            var response = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Post, Url)
            {
                RequestUri = new Uri($"{Url}/api/namespaces/test-namespace/test-module/TestInheritedClass"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(inheritedInstance, InheritingType)
            });

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync(InheritingType);
            Assert.NotNull(content);
            var value = response.Headers.GetValues("X-Identifier").First();
            Assert.NotNull(value);
        }
    }
}