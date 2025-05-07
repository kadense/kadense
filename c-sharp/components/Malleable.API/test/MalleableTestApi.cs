using System.Net.Http.Json;
using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;

namespace Kadense.Malleable.API.Tests {
    public class MalleableTestApi : MalleableApiBase
    {
        protected override async Task ProcessPostAsync<T>(HttpContext context, T content)
        {
            Assert.NotNull(content);
            Assert.Equal("TestInheritedClass", content.GetType().Name);
            
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync<T>(content);
            await context.Response.CompleteAsync();
        }
    }
}