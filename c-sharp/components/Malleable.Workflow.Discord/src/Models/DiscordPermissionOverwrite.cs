using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordPermissionOverwrite : MalleableBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("allow")]
    public string Allow { get; set; } = string.Empty;

    [JsonPropertyName("deny")]
    public string Deny { get; set; } = string.Empty;
}