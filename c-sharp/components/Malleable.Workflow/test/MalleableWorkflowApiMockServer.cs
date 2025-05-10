using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Kadense.Malleable.Workflow.Extensions;

namespace Kadense.Malleable.Workflow.Tests
{
    public class MalleableWorkflowApiMockServer : IDisposable
    {
        public IWebHost? Host { get; set; }


        public void Start(ITestOutputHelper? testOutput, MalleableWorkflowContext workflowContext)
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
                    cfg.MapWorkflow(workflowContext);
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