using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordResolvedData : MalleableBase
{
    [JsonPropertyName("users")]
    public Dictionary<string, DiscordUser> Users { get; set; } = new();

    [JsonPropertyName("members")]
    public Dictionary<string, DiscordGuildMember> Members { get; set; } = new();
    [JsonPropertyName("channels")]
    public Dictionary<string, DiscordChannel> Channels { get; set; } = new();
    [JsonPropertyName("messages")]
    public Dictionary<string, DiscordMessage> Messages { get; set; } = new();

    [JsonPropertyName("attachments")]
    public Dictionary<string, DiscordAttachment> Attachments { get; set; } = new();

    [JsonPropertyName("roles")]
    public Dictionary<string, DiscordRole> Roles { get; set; } = new();
}