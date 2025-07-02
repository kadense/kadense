using System.Text.Json.Serialization;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

public class DiscordCommandOption : MalleableBase
{
    [JsonPropertyName("type")]
    public int? Type { get; set; } = 3; // 1 for SUB_COMMAND, 2 for SUB_COMMAND_GROUP, 3 for STRING, etc.

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("options")]
    public List<DiscordCommandOption>? Options { get; set; } = new List<DiscordCommandOption>();

    [JsonPropertyName("required")]
    public bool? Required { get; set; } = false;
}