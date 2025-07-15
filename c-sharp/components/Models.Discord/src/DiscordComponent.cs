using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DiscordActionRowComponent), typeDiscriminator: 1)]
[JsonDerivedType(typeof(DiscordButtonComponent), typeDiscriminator: 2)]
[JsonDerivedType(typeof(DiscordStringSelectComponent), typeDiscriminator: 3)]
[JsonDerivedType(typeof(DiscordTextInputComponent), typeDiscriminator: 4)]
[JsonDerivedType(typeof(DiscordUserSelectComponent), typeDiscriminator: 5)]
[JsonDerivedType(typeof(DiscordRoleSelectComponent), typeDiscriminator: 6)]
[JsonDerivedType(typeof(DiscordMentionableSelectComponent), typeDiscriminator: 7)]
[JsonDerivedType(typeof(DiscordChannelSelectComponent), typeDiscriminator: 8)]
[JsonDerivedType(typeof(DiscordSectionComponent), typeDiscriminator: 9)]
[JsonDerivedType(typeof(DiscordTextDisplayComponent), typeDiscriminator: 10)]
[JsonDerivedType(typeof(DiscordThumbnailComponent), typeDiscriminator: 11)]
[JsonDerivedType(typeof(DiscordMediaGalleryComponent), typeDiscriminator: 12)]
[JsonDerivedType(typeof(DiscordFileComponent), typeDiscriminator: 13)]
[JsonDerivedType(typeof(DiscordSeparatorComponent), typeDiscriminator: 14)]
[JsonDerivedType(typeof(DiscordContainerComponent), typeDiscriminator: 17)]

public class DiscordComponent : MalleableBase
{
    public DiscordComponent()
    {
    
    }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}


[JsonDerivedType(typeof(DiscordButtonComponent), typeDiscriminator: 2)]
[JsonDerivedType(typeof(DiscordThumbnailComponent), typeDiscriminator: 11)]
public abstract class DiscordAccessoryComponent : DiscordComponent
{
    public DiscordAccessoryComponent() : base()
    {

    }
}

public class DiscordActionRowComponent : DiscordComponent
{
    public DiscordActionRowComponent() : base()
    {
    }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public List<DiscordComponent>? Components { get; set; }
}

public class DiscordButtonComponent : DiscordAccessoryComponent
{
    public DiscordButtonComponent() : base()
    {
    }

    [JsonPropertyName("style")]
    public int? Style { get; set; } = 1; // Primary by default

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
    public bool? Disabled { get; set; } = false;
}

public class DiscordStringSelectComponent : DiscordComponent
{
    public DiscordStringSelectComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; } = 1;

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; } = 1;

    [JsonPropertyName("options")]
    public List<DiscordCommandOptionChoice>? Options { get; set; }

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; } = false;
}

public class DiscordTextInputComponent : DiscordComponent
{
    public DiscordTextInputComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("style")]
    public int? Style { get; set; } = 1; // Short by default

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("min_length")]
    public int? MinLength { get; set; } = 0;

    [JsonPropertyName("max_length")]
    public int? MaxLength { get; set; } = 4000;

    [JsonPropertyName("required")]
    public bool? Required { get; set; } = true;
}

public class DiscordUserSelectComponent : DiscordComponent
{
    public DiscordUserSelectComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; } = 0;

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; } = 25;

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; } = false;
}

public class DiscordRoleSelectComponent : DiscordComponent
{
    public DiscordRoleSelectComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; } = 0;

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; } = 25;

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; } = false;
}

public class DiscordMentionableSelectComponent : DiscordComponent
{
    public DiscordMentionableSelectComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; } = 0;

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; } = 25;

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; } = false;
}

public class DiscordChannelSelectComponent : DiscordComponent
{
    public DiscordChannelSelectComponent() : base()
    {
    }

    [JsonPropertyName("custom_id")]
    public string? CustomId { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("channel_types")]
    public List<int>? ChannelTypes { get; set; }

    [JsonPropertyName("min_values")]
    public int? MinValues { get; set; } = 0;

    [JsonPropertyName("max_values")]
    public int? MaxValues { get; set; } = 25;

    [JsonPropertyName("disabled")]
    public bool? Disabled { get; set; } = false;
}

public class DiscordSectionComponent : DiscordComponent
{
    public DiscordSectionComponent() : base()
    {
    }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public List<DiscordComponent>? Components { get; set; }

    [JsonPropertyName("accessory")]
    public DiscordAccessoryComponent? Accessory { get; set; }
}

public class DiscordTextDisplayComponent : DiscordComponent
{
    public DiscordTextDisplayComponent() : base()
    {
    }

    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

public class DiscordThumbnailComponent : DiscordAccessoryComponent
{
    public DiscordThumbnailComponent() : base()
    {
    }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("media")]
    public DiscordUnfurledMediaItem? Media { get; set; }

}

public class DiscordUnfurledMediaItem : MalleableBase
{   
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("proxy_url")]
    public string? ProxyUrl { get; set; }

    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("attachment_id")]
    public string? AttachmentId { get; set; }
}

public class DiscordMediaGalleryComponent : DiscordComponent
{
    public DiscordMediaGalleryComponent() : base()
    {
    }

    [JsonPropertyName("items")]
    public List<DiscordMediaGalleryItem>? Items { get; set; }
}

public class DiscordMediaGalleryItem : MalleableBase
{
    [JsonPropertyName("media")]
    public DiscordUnfurledMediaItem? Media { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("spoiler")]
    public bool Spoiler { get; set; } = false;
}

public class DiscordFileComponent : DiscordComponent
{
    public DiscordFileComponent() : base()
    {
    }

    [JsonPropertyName("file")]
    public DiscordUnfurledMediaItem? File { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("spoiler")]
    public bool? Spoiler { get; set; } = false;

}

public class DiscordSeparatorComponent : DiscordComponent
{
    public DiscordSeparatorComponent() : base()
    {
    }

    [JsonPropertyName("divider")]
    public bool? Divider { get; set; } = true;

    [JsonPropertyName("spacing")]
    public int? Spacing { get; set; } = 1; // Default spacing is
}

public class DiscordContainerComponent : DiscordComponent
{
    public DiscordContainerComponent() : base()
    {
    }

    [JsonPropertyName("components")]
    [JsonConverter(typeof(DiscordComponentListConverter))]
    public List<DiscordComponent>? Components { get; set; } = new List<DiscordComponent>();

    [JsonPropertyName("accent_color")]
    public int? AccentColor { get; set; }

    [JsonPropertyName("spoiler")]
    public bool? Spoiler { get; set; } = false;
}