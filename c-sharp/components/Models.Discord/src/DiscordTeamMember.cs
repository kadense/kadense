using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordTeamMember : MalleableBase
{
    [JsonPropertyName("membership_state")]
    public int? MembershipState { get; set; }

    [JsonPropertyName("team_id")]
    public string? TeamId { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; }

    [JsonPropertyName("role")]
    public string? Role { get; set; }
}