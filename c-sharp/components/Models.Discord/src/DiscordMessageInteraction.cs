using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordMessageInteraction : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; }

    [JsonPropertyName("member")]
    public DiscordGuildMember? Member { get; set; }
}