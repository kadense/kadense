using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordApplicationIntegrationTypeConfiguration : MalleableBase
{
    [JsonPropertyName("oauth2_install_params")]
    public DiscordInstallParams? Oauth2InstallParams { get; set; }
}