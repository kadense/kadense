using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordInstallParams : MalleableBase
{
    [JsonPropertyName("scopes")]
    public List<string>? Scopes { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }
}