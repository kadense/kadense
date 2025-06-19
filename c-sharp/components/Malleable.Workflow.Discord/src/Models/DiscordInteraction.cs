using System.Text.Json.Serialization;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Discord.Models;

[MalleableClass("malleable", "discord", "DiscordInteraction")]
public class DiscordInteraction : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("data")]
    public DiscordInteractionData? Data { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("guild")]
    public DiscordGuild? Guild { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("channel")]
    public DiscordChannel? Channel { get; set; }

    [JsonPropertyName("member")]
    public DiscordGuildMember? Member { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("version")]
    public int? Version { get; set; }

    [JsonPropertyName("message")]
    public DiscordMessage? Message { get; set; }

    [JsonPropertyName("app_permissions")]
    public string? AppPermissions { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("guild_locale")]
    public string? GuildLocale { get; set; }

    [JsonPropertyName("entitlements")]
    public List<DiscordEntitlement> Entitlements { get; set; } = new List<DiscordEntitlement>();

    [JsonPropertyName("authorizing_integration_owners")]
    public Dictionary<int, string> AuthorizingIntegrationOwners { get; set; } = new Dictionary<int, string>();

    [JsonPropertyName("context")]
    public int? Context { get; set; }

    [JsonPropertyName("attachment_size_limit")]
    public int? AttachmentSizeLimit { get; set; }
}