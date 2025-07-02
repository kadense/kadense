using System.Text.Json.Serialization;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

public class DiscordCommand : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; } = 1; // Default to 1 (CHAT_INPUT)

    [JsonPropertyName("options")]
    public List<DiscordCommandOption>? Options { get; set; } = new List<DiscordCommandOption>();
}