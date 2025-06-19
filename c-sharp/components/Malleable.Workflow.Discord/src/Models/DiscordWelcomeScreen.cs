using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordWelcomeScreen : MalleableBase
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("welcome_channels")]
    public List<DiscordWelcomeChannel>? WelcomeChannels { get; set; }

}