using System.Text.Json.Serialization;
using Kadense.Malleable.Reflection;

namespace Kadense.Models.Discord;

[MalleableClass("malleable", "discord", "DiscordInteractionResponse")]
public class DiscordInteractionResponse : MalleableBase
{
    [JsonPropertyName("type")]
    public int Type { get; set; } = 4;

    [JsonPropertyName("data")]
    public DiscordInteractionResponseData? Data { get; set; }
}