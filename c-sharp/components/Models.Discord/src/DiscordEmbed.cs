using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEmbed : MalleableBase
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("color")]
    public int? Color { get; set; }

    [JsonPropertyName("footer")]
    public DiscordEmbedFooter? Footer { get; set; }

    [JsonPropertyName("image")]
    public DiscordEmbedImage? Image { get; set; }

    [JsonPropertyName("thumbnail")]
    public DiscordEmbedImage? Thumbnail { get; set; }

    [JsonPropertyName("video")]
    public DiscordEmbedImage? Video { get; set; }

    [JsonPropertyName("provider")]
    public DiscordEmbedProvider? Provider { get; set; }

    [JsonPropertyName("author")]
    public DiscordEmbedAuthor? Author { get; set; }

    [JsonPropertyName("fields")]
    public List<DiscordEmbedField>? Fields { get; set; }
}