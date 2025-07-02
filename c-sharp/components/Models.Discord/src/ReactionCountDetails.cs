using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class ReactionCountDetails : MalleableBase
{
    [JsonPropertyName("burst")]
    public int? Burst { get; set; }

    [JsonPropertyName("normal")]
    public int? Normal { get; set; }
}