using System.Text.Json.Serialization;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Discord.Models;

[MalleableClass("malleable", "discord", "DiscordInteractionResponse")]
public class DiscordInteractionResponse : MalleableBase
{
    [JsonPropertyName("type")]
    public int Type { get; set; } = 4;

    [JsonPropertyName("data")]
    public DiscordInteractionResponseData? Data { get; set; }
}