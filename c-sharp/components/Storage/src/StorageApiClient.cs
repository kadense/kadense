using System.Net;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kadense.Storage;

public class StorageApiClient
{
    public StorageApiClient(string baseUrl)
    {
        BaseUrl = baseUrl;
        Client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public ILogger Logger { get; set; } = NullLogger.Instance;
    public string BaseUrl { get; }
    public HttpClient Client { get; }

    public async Task<byte[]> ReadChunkAsync(string url)
    {
        using (var response = await Client.GetAsync(url))
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }

    public async Task<string> WriteChunkAsync(string url, ByteArrayContent content)
    {
        using (var response = await Client.PostAsync(url, content))
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

    public async Task DeleteChunkAsync(string url)
    {
        using (var response = await Client.DeleteAsync(url))
        {
            response.EnsureSuccessStatusCode();
        }
    }


    public async Task<StoredFile?> ReadIndexAsync(string path)
    {
        var fullPath = Path.Combine(BaseUrl, "index", path);
        using (var response = await Client.GetAsync(fullPath))
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StoredFile>();
        }
    }

    public async Task WriteIndexAsync(string path, StoredFile file)
    {
        var fullPath = Path.Combine(BaseUrl, "index", path);
        using (var response = await Client.PostAsync(fullPath, JsonContent.Create(file)))
            response.EnsureSuccessStatusCode();
    }
    
    
    public async Task DeleteIndexAsync(string path)
    {
        var fullPath = Path.Combine(BaseUrl, "index", path);
        using (var response = await Client.DeleteAsync(fullPath))
        {
            response.EnsureSuccessStatusCode();
        }
    }
}
