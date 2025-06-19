using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordInteractionOptions : MalleableBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("options")]
    public List<DiscordInteractionOptions>? Options { get; set; }

    [JsonPropertyName("focused")]
    public bool? Focused { get; set; }
}