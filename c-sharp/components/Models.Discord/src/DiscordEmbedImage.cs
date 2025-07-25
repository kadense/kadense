using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEmbedImage : MalleableBase
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("proxy_url")]
    public string? ProxyUrl { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }
    
}