using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordRoleTag : MalleableBase
{
    [JsonPropertyName("bot_id")]
    public string? BotId { get; set; }

    [JsonPropertyName("integration_id")]
    public string? IntegrationId { get; set; }

    [JsonPropertyName("premium_subscriber")]
    public bool? PremiumSubscriber { get; set; }

    [JsonPropertyName("subscription_listing_id")]
    public string? SubscriptionListingId { get; set; }

    [JsonPropertyName("available_for_purchase")]
    public bool? AvailableForPurchase { get; set; }

    [JsonPropertyName("guild_connections")]
    public bool? GuildConnections { get; set; }
}