using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordMessageSnapshot : MalleableBase
{
    [JsonPropertyName("message")]
    public DiscordMessage? Message { get; set; }
}