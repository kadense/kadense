using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordGuildMember : MalleableBase
{
    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; }

    [JsonPropertyName("nick")]
    public string? Nickname { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }

    [JsonPropertyName("joined_at")]
    public string? JoinedAt { get; set; }

    [JsonPropertyName("premium_since")]
    public string? PremiumSince { get; set; }

    [JsonPropertyName("deaf")]
    public bool? Deaf { get; set; }

    [JsonPropertyName("mute")]
    public bool? Mute { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }

    [JsonPropertyName("pending")]
    public bool? Pending { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }

    [JsonPropertyName("communication_disabled_until")]
    public string? CommunicationDisabledUntil { get; set; }

    [JsonPropertyName("avatar_decoration_data")]
    public DiscordAvatarDecorationData? AvatarDecorationData { get; set; }
}