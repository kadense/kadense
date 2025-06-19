using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class DiscordTeam : MalleableBase
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("owner_user_id")]
    public string? OwnerUserId { get; set; }
    
    [JsonPropertyName("members")]
    public List<DiscordTeamMember>? Members { get; set; }
}