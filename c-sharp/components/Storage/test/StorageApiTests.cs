using System;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Kadense.Storage.Tests;

public class StorageApiTests : KadenseTest
{
    public StorageApiTests(ITestOutputHelper output) : base(output)
    {

    }

    [TestOrder(1)]
    [Fact]
    public async Task TestStorageChunkUploadApi()
    {
        Random random = new Random();
        var mockServer = new MockFileServer();
        var baseUrl = new Uri(mockServer.Start(Output, 5, 2));
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };

        var byteArray = new byte[2048];
        random.NextBytes(byteArray);
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/chunks/test-chunk-upload.json/1/12345"),
            Method = HttpMethod.Post,
            Content = new ByteArrayContent(byteArray)
        });
        response.EnsureSuccessStatusCode();
    }

    [TestOrder(2)]
    [Fact]
    public async Task TestStorageFileListApi()
    {
        Random random = new Random();
        var mockServer = new MockFileServer();
        var baseUrl = new Uri(mockServer.Start(Output, 5, 2));
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };

        var byteArray = new byte[2048];
        random.NextBytes(byteArray);
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/files/123/test-file-list.json"),
            Method = HttpMethod.Post,
            Content = new ByteArrayContent(byteArray)
        });
        response.EnsureSuccessStatusCode();

        response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/files"),
            Method = HttpMethod.Get
        });
        response.EnsureSuccessStatusCode();
        var results = await response.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(results);
        Assert.Contains("123/test-file-list.json", results);
    }

    [TestOrder(3)]
    [Fact]
    public async Task TestStorageFileUploadApi()
    {
        Random random = new Random();
        var mockServer = new MockFileServer();
        var baseUrl = new Uri(mockServer.Start(Output, 5, 2));
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };

        var byteArray = new byte[2048];
        random.NextBytes(byteArray);
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/files/123/test-file-upload.json"),
            Method = HttpMethod.Post,
            Content = new ByteArrayContent(byteArray)
        });
        response.EnsureSuccessStatusCode();
    }

    [TestOrder(4)]
    [Fact]
    public async Task TestStorageFileUploadAndDownloadApi()
    {
        Random random = new Random();
        var mockServer = new MockFileServer();
        var baseUrl = new Uri(mockServer.Start(Output, 5, 2));
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };

        var byteArray = new byte[2048];
        random.NextBytes(byteArray);

        // Upload the file
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/files/123/test-file-upload-download.json"),
            Method = HttpMethod.Post,
            Content = new ByteArrayContent(byteArray)
        });
        response.EnsureSuccessStatusCode();

        // Download the file
        response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/files/123/test-file-upload-download.json"),
            Method = HttpMethod.Get
        });
        response.EnsureSuccessStatusCode();
        var downloadedBytes = await response.Content.ReadAsByteArrayAsync();
        Assert.NotNull(downloadedBytes);
        Assert.Equal(byteArray.Length, downloadedBytes.Length);
        for (int i = 0; i < byteArray.Length; i++)
        {
            Assert.Equal(byteArray[i], downloadedBytes[i]);
        }
    }


    [TestOrder(5)]
    [Fact]
    public async Task TestStorageChunkUploadAndDeleteApi()
    {
        Random random = new Random();
        var mockServer = new MockFileServer();
        var baseUrl = new Uri(mockServer.Start(Output, 5, 2));
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };

        var byteArray = new byte[2048];
        random.NextBytes(byteArray);

        // upload the chunk
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/chunks/test-chunk-upload-and-delete.json/1/12345"),
            Method = HttpMethod.Post,
            Content = new ByteArrayContent(byteArray)
        });
        response.EnsureSuccessStatusCode();
        
        // delete the chunk
        response = await client.SendAsync(new HttpRequestMessage()
        {
            RequestUri = new Uri($"{baseUrl}/chunks/test-chunk-upload-and-delete.json/1/12345"),
            Method = HttpMethod.Delete
        });
        response.EnsureSuccessStatusCode();
    }
}
