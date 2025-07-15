using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordMessage : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("author")]
    public DiscordUser? Author { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("edited_timestamp")]
    public string? EditedTimestamp { get; set; }

    [JsonPropertyName("tts")]
    public bool? Tts { get; set; }

    [JsonPropertyName("mention_everyone")]
    public bool? MentionEveryone { get; set; }

    [JsonPropertyName("mentions")]
    public List<DiscordUser>? Mentions { get; set; }

    [JsonPropertyName("mention_roles")]
    public List<string>? MentionRoles { get; set; }

    [JsonPropertyName("mention_channels")]
    public List<DiscordChannelMention>? MentionChannels { get; set; }

    [JsonPropertyName("attachments")]
    public List<DiscordAttachment>? Attachments { get; set; }

    [JsonPropertyName("embeds")]
    public List<DiscordEmbed>? Embeds { get; set; }

    [JsonPropertyName("reactions")]
    public List<DiscordReaction>? Reactions { get; set; }

    [JsonPropertyName("nonce")]
    public string? Nonce { get; set; }

    [JsonPropertyName("pinned")]
    public bool? Pinned { get; set; }

    [JsonPropertyName("webhook_id")]
    public string? WebhookId { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("activity")]
    public DiscordMessageActivity? Activity { get; set; }

    [JsonPropertyName("application")]
    public DiscordApplication? Application { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }

    [JsonPropertyName("message_reference")]
    public DiscordMessageReference? MessageReference { get; set; }

    [JsonPropertyName("message_snapshots")]
    public List<DiscordMessageSnapshot>? MessageSnapshots { get; set; }

    [JsonPropertyName("referenced_message")]
    public DiscordMessage? ReferencedMessage { get; set; }

    [JsonPropertyName("interaction_metadata")]
    public DiscordInteractionMetadata? InteractionMetadata { get; set; }

    [JsonPropertyName("interaction")]
    public DiscordMessageInteraction? Interaction { get; set; }

    [JsonPropertyName("thread")]
    public DiscordChannel? Thread { get; set; }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public List<DiscordComponent>? Components { get; set; }

    [JsonPropertyName("sticker_items")]
    public List<DiscordStickerItem>? StickerItems { get; set; }

    [JsonPropertyName("stickers")]
    public List<DiscordSticker>? Stickers { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("role_subscription_data")]
    public DiscordRoleSubscriptionData? RoleSubscriptionData { get; set; }

    [JsonPropertyName("resolved")]
    public DiscordResolvedData? Resolved { get; set; }

    [JsonPropertyName("poll")]
    public DiscordPoll? Poll { get; set; }

    [JsonPropertyName("call")]
    public DiscordCall? Call { get; set; }
}