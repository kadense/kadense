using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordMessageSnapshot : MalleableBase
{
    [JsonPropertyName("message")]
    public DiscordMessage? Message { get; set; }
}