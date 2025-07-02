using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordRoleSubscriptionData : MalleableBase
{
    [JsonPropertyName("role_subscription_listing_id")]
    public string? RoleSubscriptionListingId { get; set; }

    [JsonPropertyName("tier_name")]
    public string? TierName { get; set; }

    [JsonPropertyName("total_months_subscribed")]
    public int? TotalMonthsSubscribed { get; set; }

    [JsonPropertyName("is_renewal")]
    public bool? IsRenewal { get; set; }
}