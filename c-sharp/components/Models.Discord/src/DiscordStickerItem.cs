using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordStickerItem : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("format_type")]
    public int? FormatType { get; set; }
}