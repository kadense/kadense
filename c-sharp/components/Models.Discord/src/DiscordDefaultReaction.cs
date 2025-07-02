using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordDefaultReaction : MalleableBase
{
    [JsonPropertyName("emoji_id")]
    public string? EmojiId { get; set; }

    [JsonPropertyName("emoji_name")]
    public string? EmojiName { get; set; }
}