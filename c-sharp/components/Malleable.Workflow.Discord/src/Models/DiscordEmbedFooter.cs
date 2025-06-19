using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordEmbedFooter : MalleableBase
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }

    [JsonPropertyName("proxy_icon_url")]
    public string? ProxyIconUrl { get; set; }
}