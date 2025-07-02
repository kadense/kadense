using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordAvatarDecorationData : MalleableBase
{
    [JsonPropertyName("asset")]
    public string? Asset { get; set; }

    [JsonPropertyName("sku_id")]
    public string? SkuId { get; set; }
}