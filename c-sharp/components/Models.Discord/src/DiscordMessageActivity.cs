using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordMessageActivity : MalleableBase
{
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("party_id")]
    public string? PartyId { get; set; }
}