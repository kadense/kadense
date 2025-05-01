using k8s;
using Kadense.Client.Kubernetes;
using Kadense.Models.Kubernetes.Tests;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Kadense.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Kadense.Client.Kubernetes.Tests {
    public class MockedKubeApiServer : IDisposable
    {
        public IWebHost? Host { get; set; }    

        public Func<Task<string>> OnListNamespacesAsync { get; set; } = () => Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ListNamespaces.json"));
        public Func<string, Task<string>> OnReadNamespaceAsync { get; set; } = (x) => Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadNamespaces.json"));
        public Func<string,string,string,string,HttpContext, Task> OnListNamespacedObjectAsync { get; set; } = async (apiGroup, version, @namespace, kind, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ListNamespacedObject.json");
            await ctx.Response.WriteAsync(value);
        };

        public Func<string,string,string,string,HttpContext, Task> OnWatchNamespacedObjectAsync { get; set; } = async (apiGroup, version, @namespace, kind, ctx) => {
            var watchCreated = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchCoreCreated.json");
            var watchModified = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchCoreModified.json");
            var watchDeleted = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchCoreDeleted.json");
            
            foreach (var watchResponse in new string[] { watchCreated, watchModified, watchDeleted} )
            {
                var watchResponseAsString = watchResponse;
                await ctx.Response.WriteAsync(watchResponseAsString);
                await Task.Delay(100);
            }
        };

        public Func<string,string,string,string,string,Task<string>> OnReadNamespacedObjectAsync { get; set; } = (apiGroup, version, @namespace, kind, name) => Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadCoreObject.json"));
        public Func<string,string,string,string,string,HttpContext,Task> OnPutNamespacedObjectAsync { get; set; } = async (apiGroup, version, @namespace, kind, name, ctx) => 
        {   
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadCoreObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };
        public Func<string,string,string,string,HttpContext,Task> OnPostNamespacedObjectAsync { get; set; } = async (apiGroup, version, @namespace, kind, ctx) => 
        {   
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadCoreObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        public Func<string,string,string,string,string,HttpContext,Task> OnDeleteNamespacedObjectAsync { get; set; } = async (apiGroup, version, @namespace, kind, name, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadCoreObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        // Cluster Events
        public Func<string,string,string,HttpContext, Task> OnListClusterObjectAsync { get; set; } = async (apiGroup, version, kind, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ListClusterObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        public Func<string,string,string,HttpContext, Task> OnWatchClusterObjectAsync { get; set; } = async (apiGroup, version, kind, ctx) => {
            var watchCreated = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchClusterCreated.json");
            var watchModified = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchClusterModified.json");
            var watchDeleted = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.WatchClusterDeleted.json");
            ctx.Response.ContentType = "application/json";
            
            foreach (var watchResponse in new string[] { watchCreated, watchModified, watchDeleted} )
            {
                var watchResponseAsString = watchResponse;
                await ctx.Response.WriteAsync(watchResponseAsString);
                await Task.Delay(100);
            }
        };

        public Func<string,string,string,string,Task<string>> OnReadClusterObjectAsync { get; set; } = (apiGroup, version, kind, name) => Task.FromResult<string>(KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadClusterObject.json"));
        public Func<string,string,string,HttpContext,Task> OnPostClusterObjectAsync { get; set; } = async (apiGroup, version, kind, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadClusterObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        public Func<string,string,string,string,HttpContext,Task> OnPutClusterObjectAsync { get; set; } = async (apiGroup, version, kind, name, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadClusterObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        public Func<string,string,string,string,HttpContext,Task> OnDeleteClusterObjectAsync { get; set; } = async (apiGroup, version, kind, name, ctx) => {
            var value = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Client.Kubernetes.Tests.Resources.ReadClusterObject.json");
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(value);
            await ctx.Response.CompleteAsync();
        };

        public void Start(ITestOutputHelper? testOutput = null)
        {
            Host = WebHost.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(app => {
                
                app
                .UseRouting()
                .UseEndpoints(cfg => {
                    cfg.MapGet($"/api/v1/namespaces", () => this.OnListNamespacesAsync);
                    cfg.MapGet($"/api/v1/namespaces/{{namespace}}", (string @namespace) => this.OnReadNamespaceAsync(@namespace));

                    // Namespaced Objects
                    cfg.MapGet($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}", async (string apiGroup, string version, string @namespace, string kind, [FromQuery] string? watch, HttpContext ctx) =>
                    {
                        if (watch == null)
                        {
                            await this.OnListNamespacedObjectAsync(apiGroup, version, @namespace, kind, ctx);
                        }
                        else
                        {
                            await this.OnWatchNamespacedObjectAsync(apiGroup, version, @namespace, kind, ctx);
                        }
                    });

                    cfg.MapGet($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string apiGroup, string version, string @namespace, string kind, string name) => this.OnReadNamespacedObjectAsync(apiGroup, version, @namespace, kind, name));

                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));
                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/status", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));
                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/scale", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/status", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/scale", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));

                    cfg.MapPost($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}", (string apiGroup, string version, string @namespace, string kind, HttpContext ctx) => this.OnPostNamespacedObjectAsync(apiGroup, version, @namespace, kind, ctx));
                    
                    cfg.MapDelete($"/apis/{{apiGroup}}/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string apiGroup, string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnDeleteNamespacedObjectAsync(apiGroup, version, @namespace, kind, name, ctx));

                    // Core Resources
                    
                    cfg.MapGet($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}", async (string version, string @namespace, string kind, [FromQuery] string? watch, HttpContext ctx) =>
                    {
                        if (watch == null)
                        {
                            await this.OnListNamespacedObjectAsync("", version, @namespace, kind, ctx);
                        }
                        else
                        {
                            await this.OnWatchNamespacedObjectAsync("", version, @namespace, kind, ctx);
                        }
                    });

                    cfg.MapGet($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string version, string @namespace, string kind, string name) => this.OnReadNamespacedObjectAsync("", version, @namespace, kind, name));

                    cfg.MapPatch($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));
                    cfg.MapPatch($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/status", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));
                    cfg.MapPatch($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/scale", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/status", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));
                    cfg.MapPut($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}/scale", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnPutNamespacedObjectAsync("", version, @namespace, kind, name, ctx));

                    cfg.MapPost($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}", (string version, string @namespace, string kind, HttpContext ctx) => this.OnPostNamespacedObjectAsync("", version, @namespace, kind, ctx));
                    
                    cfg.MapDelete($"/api/{{version}}/namespaces/{{namespace}}/{{kind}}/{{name}}", (string version, string @namespace, string kind, string name, HttpContext ctx) => this.OnDeleteNamespacedObjectAsync("", version, @namespace, kind, name, ctx));


                    // Cluster Objects
                    cfg.MapGet($"/apis/{{apiGroup}}/{{version}}/{{kind}}", async (string apiGroup, string version, string kind, [FromQuery] string? watch, HttpContext ctx) =>
                    {
                        if (watch == null)
                        {
                            await this.OnListClusterObjectAsync(apiGroup, version, kind, ctx);
                        }
                        else
                        {
                            await this.OnWatchClusterObjectAsync(apiGroup, version, kind, ctx);
                        }
                    });

                    cfg.MapGet($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}", (string apiGroup, string version, string kind, string name) => this.OnReadClusterObjectAsync(apiGroup, version, kind, name));

                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));
                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}/status", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));
                    cfg.MapPatch($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}/scale", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}/status", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));
                    cfg.MapPut($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}/scale", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnPutClusterObjectAsync(apiGroup, version, kind, name, ctx));

                    cfg.MapPost($"/apis/{{apiGroup}}/{{version}}/{{kind}}", (string apiGroup, string version, string kind, HttpContext ctx) => this.OnPostClusterObjectAsync(apiGroup, version, kind, ctx));
                    
                    cfg.MapDelete($"/apis/{{apiGroup}}/{{version}}/{{kind}}/{{name}}", (string apiGroup, string version, string kind, string name, HttpContext ctx) => this.OnDeleteClusterObjectAsync(apiGroup, version, kind, name, ctx));

                });
            })
            .UseKestrel(options => { options.Listen(System.Net.IPAddress.Loopback, 0, (_) => {}); })
            .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    if(testOutput != null)
                    {
                        logging.AddProvider(new KadenseTestLoggerProvider(testOutput));
                    }
                    else
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                    }
                })
            .Build();


            Host.Start();
        }

        public void Dispose()
        {
            if (Host != null)
            {
                Host.StopAsync();
                Host.WaitForShutdown();
                Host.Dispose();
            }
        }

        public k8s.Kubernetes CreateClient()
        {
            var url = Host!
                .ServerFeatures
                .Get<IServerAddressesFeature>()!
                .Addresses
                .Select(a => new Uri(a))
                .First();;

            return new k8s.Kubernetes(new KubernetesClientConfiguration
            {
                Host = url.ToString(),
            });
        }
    }
}