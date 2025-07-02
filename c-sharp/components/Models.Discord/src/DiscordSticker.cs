using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordSticker : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("pack_id")]
    public string? PackId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("tags")]
    public string? Tags { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("format_type")]
    public int? FormatType { get; set; }

    [JsonPropertyName("available")]
    public bool? Available { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; } // Assuming DiscordUser is defined elsewhere

    [JsonPropertyName("sort_value")]
    public int? SortValue { get; set; }
}