using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordIncidentsData : MalleableBase
{
    [JsonPropertyName("invites_disabled_until")]
    public string? InvitesDisabledUntil { get; set; }

    [JsonPropertyName("dms_disabled_until")]
    public string? DmsDisabledUntil { get; set; }

    [JsonPropertyName("dm_spam_detected_at")]
    public string? DmSpamDetectedAt { get; set; }

    [JsonPropertyName("raid_detected_at")]
    public string? RaidDetectedAt { get; set; }
}