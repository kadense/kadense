using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordInteractionResponseData : MalleableBase
{
    [JsonPropertyName("tts")]
    public bool? Tts { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("embeds")]
    public List<DiscordEmbed>? Embeds { get; set; }

    [JsonPropertyName("flags")]
    public int? Flags { get; set; }

    //[JsonPropertyName("allowed_mentions")]
    //public DiscordAllowedMentions? AllowedMentions { get; set; }

    [JsonPropertyName("attachments")]
    public List<DiscordAttachment>? Attachments { get; set; }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public List<DiscordComponent>? Components { get; set; }   
}