using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordMessageReference : MalleableBase
{
    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("fail_if_not_exists")]
    public bool? FailIfNotExists { get; set; }
    
    [JsonPropertyName("type")]
    public int? Type { get; set; }
}