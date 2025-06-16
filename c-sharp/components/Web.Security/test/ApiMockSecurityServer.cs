using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Kadense.Web.Security;


namespace Kadense.Web.Security.Tests {
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    public class ApiMockSecurityServer : IDisposable
    {
        public ApiMockSecurityServer()
        {
            ClientId = Guid.NewGuid().ToString();
            ClientSecret = Guid.NewGuid().ToString();
        }
        public IWebHost? Host { get; set; }


        public void Start(ITestOutputHelper? testOutput)
        {
            Host = WebHost.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
            })
            .Configure(app =>
            {

                app
                .UseDeveloperExceptionPage()
                .UseForwardedHeaders()
                .UseRouting()
                .UseEndpoints(cfg =>
                {
                    cfg.MapKadenseOAuthToDummyProvider();
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
        
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
    }
}