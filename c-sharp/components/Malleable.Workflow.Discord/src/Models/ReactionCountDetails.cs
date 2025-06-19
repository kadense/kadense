using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public class ReactionCountDetails : MalleableBase
{
    [JsonPropertyName("burst")]
    public int? Burst { get; set; }

    [JsonPropertyName("normal")]
    public int? Normal { get; set; }
}