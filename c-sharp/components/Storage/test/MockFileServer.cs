using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Kadense.Storage.Tests;


public class MockFileServer : IDisposable
{
    public IWebHost? Host { get; set; }

    public string Start(ITestOutputHelper? testOutput, int nodes, int replicas)
    {
        var loggingProvider = new KadenseTestLoggerProvider(testOutput!);
        var nodeUrls = new List<string>();
        var nodeClients = new List<StorageApiClient>();
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
                var nodeList = new List<string>();
                for (int i = 0; i < nodes; i++)
                {
                    var localPathInfo = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly()!.Location);
                    var localPath = localPathInfo.Directory!.FullName;
                    var logger = loggingProvider.CreateLogger($"storage-api-{i}");
                    var storageApi = new StorageApiFactory()
                        .WithLoggerProvider(loggingProvider)
                        .WithMedia(new FileSystemStorageApiMedia($"{localPath}/mnt/storage/{i}"))
                        .WithNodeDistributor(new StorageApiNodeDistributorRandom(nodeClients))
                        .WithBasePath($"/api/storage/{i}")
                        .WithMaximumChunkSize(512)
                        .Build();
                    
                    storageApi.MapRoutes(cfg);
                }
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

        var serverAddress = Host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
        if (serverAddress == null)
        {
            throw new InvalidOperationException("No address found.");
        }
        for (int i = 0; i < nodes; i++)
        {
            var nodeUrl = $"{serverAddress}/api/storage/{i}";
            nodeUrls.Add(nodeUrl);
            nodeClients.Add(new StorageApiClient(nodeUrl));
        }

        return nodeUrls.First();
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