using System.Text.Json.Serialization;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

public class DiscordWelcomeScreen : MalleableBase
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("welcome_channels")]
    public List<DiscordWelcomeChannel>? WelcomeChannels { get; set; }

}