using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordMessageActivity : MalleableBase
{
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("party_id")]
    public string? PartyId { get; set; }
}