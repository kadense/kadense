using System.Net.Http.Json;
using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable.Tests;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace Kadense.Malleable.API.Tests {
    public class IEndpointRouteBuilderExtensions : KadenseTest
    {
        public IEndpointRouteBuilderExtensions(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public async Task TestPostAsync()
        {
            var server = new MalleableApiMockServer();
            var client = new HttpClient();
            var mocker = new MalleableMockers();
            var module = mocker.MockModule();
            var assemblyBuilder = new MalleableAssemblyFactory();
            var assembly = assemblyBuilder.CreateAssembly(module);
            var inheritingType = assembly.Types["TestInheritedClass"];
            server.Start(Output, [ inheritingType ]);

            var url = server.GetUrl();
            client.BaseAddress = new Uri(url);

            var inheritedInstance = Activator.CreateInstance(inheritingType);
            Assert.NotNull(inheritedInstance);

            inheritingType.GetProperty("TestString")!.SetValue(inheritedInstance, "test1");
            inheritingType.GetProperty("TestList")!.SetValue(inheritedInstance, new List<string> { "test2", "test3" });


            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
            {
                RequestUri = new Uri($"{url}/api/namespaces/test-namespace/test-module/TestInheritedClass"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(inheritedInstance, inheritingType)
            });

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync(inheritingType);
            Assert.NotNull(content);
        }
    }
}