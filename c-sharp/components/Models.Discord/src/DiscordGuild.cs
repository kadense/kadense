using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordGuild : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("icon_hash")]
    public string? IconHash { get; set; }

    [JsonPropertyName("splash")]
    public string? Splash { get; set; }

    [JsonPropertyName("discovery_splash")]
    public string? DiscoverySplash { get; set; }

    [JsonPropertyName("owner")]
    public bool? Owner { get; set; }

    [JsonPropertyName("owner_id")]
    public string? OwnerId { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("afk_channel_id")]
    public string? AfkChannelId { get; set; }

    [JsonPropertyName("afk_timeout")]
    public int? AfkTimeout { get; set; }

    [JsonPropertyName("widget_enabled")]
    public bool? WidgetEnabled { get; set; }

    [JsonPropertyName("widget_channel_id")]
    public string? WidgetChannelId { get; set; }

    [JsonPropertyName("verification_level")]
    public int? VerificationLevel { get; set; }

    [JsonPropertyName("default_message_notifications")]
    public int? DefaultMessageNotifications { get; set; }

    [JsonPropertyName("explicit_content_filter")]
    public int? ExplicitContentFilter { get; set; }

    [JsonPropertyName("roles")]
    public List<DiscordRole>? Roles { get; set; }

    [JsonPropertyName("emojis")]
    public List<DiscordEmoji>? Emojis { get; set; }

    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }

    [JsonPropertyName("mfa_level")]
    public int? MfaLevel { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("system_channel_id")]
    public string? SystemChannelId { get; set; }

    [JsonPropertyName("system_channel_flags")]
    public int? SystemChannelFlags { get; set; }

    [JsonPropertyName("rules_channel_id")]
    public string? RulesChannelId { get; set; }

    [JsonPropertyName("max_presences")]
    public int? MaxPresences { get; set; }

    [JsonPropertyName("max_members")]
    public int? MaxMembers { get; set; }

    [JsonPropertyName("vanity_url_code")]
    public string? VanityUrlCode { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }


    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("premium_tier")]
    public int? PremiumTier { get; set; }

    [JsonPropertyName("premium_subscription_count")]
    public int? PremiumSubscriptionCount { get; set; }

    [JsonPropertyName("preferred_locale")]
    public string? PreferredLocale { get; set; }

    [JsonPropertyName("public_updates_channel_id")]
    public string? PublicUpdatesChannelId { get; set; }

    [JsonPropertyName("max_video_channel_users")]
    public int? MaxVideoChannelUsers { get; set; }

    [JsonPropertyName("max_stage_video_channel_users")]
    public int? MaxStageVideoChannelUsers { get; set; }

    [JsonPropertyName("approximate_member_count")]
    public int? ApproximateMemberCount { get; set; }

    [JsonPropertyName("approximate_presence_count")]
    public int? ApproximatePresenceCount { get; set; }

    [JsonPropertyName("welcome_screen")]
    public DiscordWelcomeScreen? WelcomeScreen { get; set; }

    [JsonPropertyName("nsfw_level")]
    public int? NsfwLevel { get; set; }

    [JsonPropertyName("stickers")]
    public List<DiscordSticker>? Stickers { get; set; }

    [JsonPropertyName("premium_progress_bar_enabled")]
    public bool? PremiumProgressBarEnabled { get; set; }

    [JsonPropertyName("safety_alerts_channel_id")]
    public string? SafetyAlertsChannelId { get; set; }

    [JsonPropertyName("incidents_data")]
    public DiscordIncidentsData? IncidentsData { get; set; }
}