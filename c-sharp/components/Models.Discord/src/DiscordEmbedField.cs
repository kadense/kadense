using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEmbedField : MalleableBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("inline")]
    public bool? Inline { get; set; }
}