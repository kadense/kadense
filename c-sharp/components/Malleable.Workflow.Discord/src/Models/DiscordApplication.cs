using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordApplication : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("rpc_origins")]
    public List<string>? RpcOrigins { get; set; }

    [JsonPropertyName("bot_public")]
    public bool? BotPublic { get; set; }

    [JsonPropertyName("bot_require_code_grant")]
    public bool? BotRequireCodeGrant { get; set; }

    [JsonPropertyName("bot")]
    public DiscordUser? Bot { get; set; }

    [JsonPropertyName("terms_of_service_url")]
    public string? TermsOfServiceUrl { get; set; }

    [JsonPropertyName("privacy_policy_url")]
    public string? PrivacyPolicyUrl { get; set; }

    [JsonPropertyName("owner")]
    public DiscordUser? Owner { get; set; }

    [JsonPropertyName("verify_key")]
    public string? VerifyKey { get; set; }

    [JsonPropertyName("team")]
    public DiscordTeam? Team { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("guild")]
    public DiscordGuild? Guild { get; set; }

    [JsonPropertyName("primary_sku_id")]
    public string? PrimarySkuId { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("cover_image")]
    public string? CoverImage { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }

    [JsonPropertyName("approximate_guild_count")]
    public int? ApproximateGuildCount { get; set; }

    [JsonPropertyName("approximate_user_install_count")]
    public int? ApproximateUserInstallCount { get; set; }

    [JsonPropertyName("redirect_uris")]
    public List<string>? RedirectUris { get; set; }

    [JsonPropertyName("interaction_endpoint_url")]
    public string? InteractionEndpointUrl { get; set; }

    [JsonPropertyName("role_connections_verification_url")]
    public string? RoleConnectionsVerificationUrl { get; set; }

    [JsonPropertyName("event_webhooks_url")]
    public string? EventWebhooksUrl { get; set; }

    [JsonPropertyName("event_webhooks_status")]
    public int? EventWebhooksStatus { get; set; }

    [JsonPropertyName("event_webhooks_types")]
    public List<string>? EventWebhooksTypes { get; set; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }

    [JsonPropertyName("install_params")]
    public DiscordInstallParams? InstallParams { get; set; }

    [JsonPropertyName("integration_types_config")]
    public Dictionary<int, int>? IntegrationTypesConfig { get; set; }

    [JsonPropertyName("custom_install_url")]
    public string? CustomInstallUrl { get; set; }
}