using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordAvatarDecorationData : MalleableBase
{
    [JsonPropertyName("asset")]
    public string? Asset { get; set; }

    [JsonPropertyName("sku_id")]
    public string? SkuId { get; set; }
}