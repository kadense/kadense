using System.Text.Json.Serialization;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

public class DiscordCommandOptionChoice : MalleableBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}