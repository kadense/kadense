using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordChannelMention : MalleableBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("guild_id")]
    public string GuildId { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}