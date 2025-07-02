using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEmbedProvider : MalleableBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}