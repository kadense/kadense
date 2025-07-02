using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordEntitlement : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("sku_id")]
    public string? SkuId { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("deleted")]
    public bool? Deleted { get; set; }

    [JsonPropertyName("starts_at")]
    public string? StartsAt { get; set; }

    [JsonPropertyName("ends_at")]
    public string? EndsAt { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("consumed")]
    public bool? Consumed { get; set; }
}