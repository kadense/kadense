using System.Security.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kadense.Storage;

public abstract class StorageApiNodeDistributor
{
    public StorageApiNodeDistributor(List<StorageApiClient> nodeClients)
    {
        NodeClients = nodeClients ?? throw new ArgumentNullException(nameof(nodeClients));
    }

    public ILogger Logger { get; set; } = NullLogger.Instance;
    protected List<StorageApiClient> NodeClients { get; }
    public StorageApi? Api { get; set; }
    public abstract Task<StorageApiClient[]> GetNodeOrderAsync();
}

public class StorageApiNodeDistributorRandom : StorageApiNodeDistributor
{
    public StorageApiNodeDistributorRandom(List<StorageApiClient> nodeClients) : base(nodeClients)
    {

    }

    public override Task<StorageApiClient[]> GetNodeOrderAsync()
    {
        var nodeClients = NodeClients.ToArray();
        Shuffle(nodeClients);
        return Task.FromResult(nodeClients);
    }

    protected void Shuffle(StorageApiClient[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}