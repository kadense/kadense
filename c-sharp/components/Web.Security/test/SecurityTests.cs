using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Kadense.Web.Security.Tests {
    public class SecurityTests : KadenseTest
    { 
        public SecurityTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestRequestToken()
        {
            var server = new ApiMockSecurityServer();
            server.Start(Output);
            var url = server.GetUrl();
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);


            /*
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
            {
                RequestUri = new Uri($"{url}/connect/token"),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(instance, instance.GetType(), options: GetJsonSerializerOptions()) 
            });            

            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);
            Assert.Contains("access_token", response.Content.ReadAsStringAsync().Result);
            */
        }
    }
}