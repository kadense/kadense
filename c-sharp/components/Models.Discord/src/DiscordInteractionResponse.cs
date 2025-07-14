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

public enum DiscordInteractionResponseType
{
    PONG = 1,
    CHANNEL_MESSAGE_WITH_SOURCE = 4,
    DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE = 5,
    DEFERRED_UPDATE_MESSAGE = 6,
    UPDATE_MESSAGE = 7,
    APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8,
    MODAL = 9,
    PREMIUM_REQUIRED = 10,
    LAUNCH_ACTIVITY = 12
}