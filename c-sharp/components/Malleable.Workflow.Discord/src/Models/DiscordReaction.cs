using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordReaction : MalleableBase
{
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("count_details")]
    public ReactionCountDetails? CountDetails { get; set; }

    [JsonPropertyName("me")]
    public bool Me { get; set; }

    [JsonPropertyName("emoji")]
    public DiscordEmoji? Emoji { get; set; }

    [JsonPropertyName("me_burst")]
    public bool? MeBurst { get; set; }

    [JsonPropertyName("burst_colors")]
    public List<string>? BurstColors { get; set; }
}