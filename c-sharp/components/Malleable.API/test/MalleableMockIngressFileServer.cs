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
using Microsoft.Extensions.DependencyInjection;

namespace Kadense.Malleable.API.Tests
{
    public class MalleableMockIngressFileServer : IDisposable
    {
        public IWebHost? Host { get; set; }


        public void Start(ITestOutputHelper? testOutput, IEnumerable<Type> malleableTypes)
        {
            Host = WebHost.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(app =>
            {

                app
                .UseRouting()
                .UseEndpoints(cfg =>
                {
                    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "test-ingress");
                    var folder = new DirectoryInfo(basePath);
                    if (folder.Exists)
                    {
                        folder.Delete(true);
                    }
                    var malleableApi = new MalleableIngressApiFileServer(basePath: basePath);
                    malleableApi.Process(cfg, malleableTypes);
                });
            })
            .UseKestrel(options => { options.Listen(System.Net.IPAddress.Loopback, 0, (_) => { }); })
            .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    if (testOutput != null)
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

        public string GetUrl()
        {
            if (Host == null)
            {
                throw new InvalidOperationException("Host is not started.");
            }

            var address = Host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
            if (address == null)
            {
                throw new InvalidOperationException("No address found.");
            }

            return address;
        }
    }
}