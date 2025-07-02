using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEmoji : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("animated")]
    public bool Animated { get; set; }

    [JsonPropertyName("available")]
    public bool Available { get; set; }

    [JsonPropertyName("managed")]
    public bool Managed { get; set; }

    [JsonPropertyName("require_colons")]
    public bool RequireColons { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; } // Assuming DiscordUser is defined elsewhere

    [JsonPropertyName("roles")]
    public List<DiscordRole>? Roles { get; set; } // Assuming roles are represented as an
    
}