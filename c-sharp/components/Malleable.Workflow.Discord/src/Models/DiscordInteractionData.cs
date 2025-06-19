using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordInteractionData : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("resolved")]
    public DiscordResolvedData? Resolved { get; set; }

    [JsonPropertyName("options")]
    public List<DiscordInteractionOptions>? Options { get; set; }
}