using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordChannel : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("parent_id")]
    public string? ParentId { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; set; }

    [JsonPropertyName("last_message_id")]
    public string? LastMessageId { get; set; }

    [JsonPropertyName("rate_limit_per_user")]
    public int? RateLimitPerUser { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }

    [JsonPropertyName("permission_overwrites")]
    public List<DiscordPermissionOverwrite>? PermissionOverwrites { get; set; }

    [JsonPropertyName("bitrate")]
    public int? Bitrate { get; set; }

    [JsonPropertyName("user_limit")]
    public int? UserLimit { get; set; }

    [JsonPropertyName("recipients")]
    public List<DiscordUser>? Recipients { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("owner_id")]
    public string? OwnerId { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("managed")]
    public bool? Managed { get; set; }

    [JsonPropertyName("last_pin_timestamp")]
    public string? LastPinTimestamp { get; set; }

    [JsonPropertyName("rtc_region")]
    public string? RtcRegion { get; set; }

    [JsonPropertyName("video_quality_mode")]
    public int? VideoQualityMode { get; set; }

    [JsonPropertyName("message_count")]
    public int? MessageCount { get; set; }

    [JsonPropertyName("thread_metadata")]
    public DiscordThreadMetadata? ThreadMetadata { get; set; }

    [JsonPropertyName("member")]
    public DiscordGuildMember? Member { get; set; }

    [JsonPropertyName("default_auto_archive_duration")]
    public int? DefaultAutoArchiveDuration { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }

    [JsonPropertyName("total_message_sent")]
    public int? TotalMessageSent { get; set; }

    [JsonPropertyName("available_tags")]
    public List<DiscordForumTag>? AvailableTags { get; set; }

    [JsonPropertyName("applied_tags")]
    public List<string>? AppliedTags { get; set; }

    [JsonPropertyName("default_reaction_emoji")]
    public DiscordDefaultReaction? DefaultReactionEmoji { get; set; }

    [JsonPropertyName("default_sort_order")]
    public int? DefaultSortOrder { get; set; }

    [JsonPropertyName("default_forum_layout")]
    public int? DefaultForumLayout { get; set; }

    [JsonPropertyName("default_thread_rate_limit_per_user")]
    public int? DefaultThreadRateLimitPerUser { get; set; }
}