using System.Text.Json.Serialization;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

public class DiscordWelcomeChannel : MalleableBase
{
    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("emoji_id")]
    public string? EmojiId { get; set; }

    [JsonPropertyName("emoji_name")]
    public string? EmojiName { get; set; }
}