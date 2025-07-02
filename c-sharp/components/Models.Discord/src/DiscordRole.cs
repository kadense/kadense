using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordRole : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public int? Color { get; set; }

    [JsonPropertyName("hoist")]
    public bool? Hoist { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("unicode_emoji")]
    public string? UnicodeEmoji { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }

    [JsonPropertyName("managed")]
    public bool? Managed { get; set; }

    [JsonPropertyName("mentionable")]
    public bool? Mentionable { get; set; }

    [JsonPropertyName("tags")]
    public List<DiscordRoleTag>? Tags { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }
}