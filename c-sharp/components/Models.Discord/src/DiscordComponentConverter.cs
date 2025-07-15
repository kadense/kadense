using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kadense.Models.Discord;

public class DiscordComponentListConverter : JsonConverter<List<DiscordComponent>>
{
    public override List<DiscordComponent>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var combinedComponents = (List<DiscordCombinedComponent>?)JsonSerializer.Deserialize(ref reader, typeof(List<DiscordCombinedComponent>), options);

        if (combinedComponents == null)
            return null;

        return combinedComponents.Select(c => DiscordComponentConverter.Convert(c)).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<DiscordComponent> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}


public class DiscordComponentConverter : JsonConverter<DiscordComponent>
{
    public override DiscordComponent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var combinedComponent = (DiscordCombinedComponent?)JsonSerializer.Deserialize(ref reader, typeof(DiscordCombinedComponent), options);

        if (combinedComponent == null)
            return null;

        return Convert(combinedComponent); 
    }

    public static DiscordComponent Convert(DiscordCombinedComponent combinedComponent)
    {
        switch (combinedComponent.Type)
        {
            case 1: // ActionRow
                return new DiscordActionRowComponent
                {
                    Components = combinedComponent.Components,
                    Id = combinedComponent.Id,
                };


            case 2: // Button
                return new DiscordButtonComponent
                {
                    Style = combinedComponent.Style,
                    Label = combinedComponent.Label,
                    CustomId = combinedComponent.CustomId,
                    Emoji = combinedComponent.Emoji,
                    Url = combinedComponent.Url,
                    SkuId = combinedComponent.SkuId,
                    Disabled = combinedComponent.Disabled,
                    Id = combinedComponent.Id,
                };
            case 3: // StringSelect
                return new DiscordStringSelectComponent
                {
                    CustomId = combinedComponent.CustomId,
                    Placeholder = combinedComponent.Placeholder,
                    MinValues = combinedComponent.MinValues,
                    MaxValues = combinedComponent.MaxValues,
                    Options = combinedComponent.Options,
                    Id = combinedComponent.Id,
                    Disabled = combinedComponent.Disabled,
                };
            case 4: // TextInput
                return new DiscordTextInputComponent
                {
                    CustomId = combinedComponent.CustomId,
                    Label = combinedComponent.Label,
                    Style = combinedComponent.Style,
                    Value = combinedComponent.Value,
                    Required = combinedComponent.Required,
                    Id = combinedComponent.Id,
                    Placeholder = combinedComponent.Placeholder,
                    MaxLength = combinedComponent.MaxLength,
                    MinLength = combinedComponent.MinLength,
                };
            case 5: // UserSelect
                return new DiscordUserSelectComponent
                {
                    CustomId = combinedComponent.CustomId,
                    Placeholder = combinedComponent.Placeholder,
                    MinValues = combinedComponent.MinValues,
                    MaxValues = combinedComponent.MaxValues,
                    Id = combinedComponent.Id,
                    Disabled = combinedComponent.Disabled,
                };
            case 6: // RoleSelect
                return new DiscordRoleSelectComponent
                {
                    CustomId = combinedComponent.CustomId,
                    Placeholder = combinedComponent.Placeholder,
                    MinValues = combinedComponent.MinValues,
                    MaxValues = combinedComponent.MaxValues,
                    Id = combinedComponent.Id,
                    Disabled = combinedComponent.Disabled,
                };
            case 7: // MentionableSelect
                return new DiscordMentionableSelectComponent
                {
                    CustomId = combinedComponent.CustomId,
                    Placeholder = combinedComponent.Placeholder,
                    MinValues = combinedComponent.MinValues,
                    MaxValues = combinedComponent.MaxValues,
                    Id = combinedComponent.Id,
                    Disabled = combinedComponent.Disabled,
                };
            case 8: // ChannelSelect
                return new DiscordChannelSelectComponent
                {
                    CustomId = combinedComponent.CustomId,
                    ChannelTypes = combinedComponent.ChannelTypes ?? [],
                    Placeholder = combinedComponent.Placeholder,
                    MinValues = combinedComponent.MinValues,
                    MaxValues = combinedComponent.MaxValues,
                    Disabled = combinedComponent.Disabled,
                    Id = combinedComponent.Id,
                };
            case 9: // Section
                return new DiscordSectionComponent
                {
                    Components = combinedComponent.Components,
                    Accessory = (DiscordAccessoryComponent?)combinedComponent.Accessory,
                    Id = combinedComponent.Id,
                };
            case 10: // TextDisplay
                return new DiscordTextDisplayComponent
                {
                    Content = combinedComponent.Content,
                    Id = combinedComponent.Id,
                };
            case 11:
                return new DiscordThumbnailComponent
                {
                    Media = combinedComponent.Media,
                    Url = combinedComponent.Url,
                    Id = combinedComponent.Id,
                };

            case 12: // Accessory
                return new DiscordMediaGalleryComponent
                {
                    Items = combinedComponent.Items,
                    Id = combinedComponent.Id,
                };

            case 13:
                return new DiscordFileComponent
                {
                    File = combinedComponent.File,
                    Spoiler = combinedComponent.Spoiler,
                    Name = combinedComponent.Name,
                    Size = combinedComponent.Size,
                    Id = combinedComponent.Id,
                };

            case 14: // Separator
                return new DiscordSeparatorComponent
                {
                    Id = combinedComponent.Id,
                    Divider = combinedComponent.Divider,
                    Spacing = combinedComponent.Spacing,
                };
            case 17: // Container
                return new DiscordContainerComponent
                {
                    Components = combinedComponent.Components,
                    AccentColor = combinedComponent.AccentColor,
                    Spoiler = combinedComponent.Spoiler,
                    Id = combinedComponent.Id,
                };
            default:
                throw new NotSupportedException($"Component type {combinedComponent.Type} is not supported.");
        }
    }

    public override void Write(Utf8JsonWriter writer, DiscordComponent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType());
    }
}