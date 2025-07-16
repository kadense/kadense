using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordInteractionData : MalleableBase, ICustomId
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(NumberOrStringConverter))]
    public string? Id { get; set; }

    
    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("resolved")]
    public DiscordResolvedData? Resolved { get; set; }

    [JsonPropertyName("options")]
    public List<DiscordInteractionOptions>? Options { get; set; }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public DiscordComponentList? Components { get; set; }
}