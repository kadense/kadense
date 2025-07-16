using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordCombinedComponent : ICustomId
{
    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public DiscordComponentList? Components { get; set; }

    [JsonPropertyName("style")]
    public int? Style { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("emoji")]
    public DiscordEmoji? Emoji { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("sku_id")]
    public string? SkuId { get; set; }

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; }

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; }

    [JsonPropertyName("options")]
    public List<DiscordCommandOptionChoice>? Options { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("channel_types")]
    public List<int>? ChannelTypes { get; set; }


    [JsonPropertyName("accessory")]
    [JsonConverter(typeof(DiscordComponentConverter))]
    public DiscordComponent? Accessory { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("media")]
    public DiscordUnfurledMediaItem? Media { get; set; }

    [JsonPropertyName("items")]
    public List<DiscordMediaGalleryItem>? Items { get; set; }

    [JsonPropertyName("spoiler")]
    public bool? Spoiler { get; set; }


    [JsonPropertyName("file")]
    public DiscordUnfurledMediaItem? File { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("min_length")]
    public int? MinLength { get; set; }

    [JsonPropertyName("max_length")]
    public int? MaxLength { get; set; }

    [JsonPropertyName("divider")]
    public bool? Divider { get; set; }

    [JsonPropertyName("spacing")]
    public int? Spacing { get; set; } 

    [JsonPropertyName("accent_color")]
    public int? AccentColor { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}