using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordInteractionMetadata : MalleableBase
{
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("user")]
    public DiscordUser? User { get; set; }

    [JsonPropertyName("authorizing_integration_owners")]
    public Dictionary<int, int>? AuthorizingIntegrationOwners { get; set; }

    [JsonPropertyName("original_response_message_id")]
    public string? OriginalResponseMessageId { get; set; }

    [JsonPropertyName("target_user")]
    public DiscordUser? TargetUser { get; set; }

    [JsonPropertyName("target_message_id")]
    public string? TargetMessageId { get; set; }
}