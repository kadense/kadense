namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordInteractionResponseDataBuilder
{
    public DiscordInteractionResponseDataBuilder(DiscordInteractionResponseBuilder parent, DiscordInteractionResponseData data)
    {
        Parent = parent;
        Data = data;
    }

    protected DiscordInteractionResponseBuilder Parent { get; }
    protected DiscordInteractionResponseData Data { get; }

    public DiscordInteractionResponseDataBuilder WithContent(string content)
    {
        Data.Content = content;
        return this;
    }

    public DiscordEmbedBuilder WithEmbed()
    {
        if (Data.Embeds == null)
            Data.Embeds = new List<DiscordEmbed>();

        var embed = new DiscordEmbed();
        Data.Embeds.Add(embed);
        return new DiscordEmbedBuilder(this, embed);
    }

    public DiscordInteractionResponseBuilder End()
    {
        return Parent;
    }

    public DiscordInteractionResponseDataBuilder WithTTS(bool tts)
    {
        Data.Tts = tts;
        return this;
    }

    public DiscordInteractionResponseDataBuilder WithFlags(int flags)
    {
        Data.Flags = flags;
        return this;
    }

    public DiscordInteractionResponseDataBuilder WithAttachments(List<DiscordAttachment> attachments)
    {
        Data.Attachments = attachments;
        return this;
    }

    public DiscordActionRowComponentBuilder<DiscordInteractionResponseDataBuilder> WithActionRowComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordActionRowComponent();
        Data.Components.Add(component);
        return new DiscordActionRowComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }
    public DiscordStringSelectComponentBuilder<DiscordInteractionResponseDataBuilder> WithStringSelectComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordStringSelectComponent();
        Data.Components.Add(component);
        return new DiscordStringSelectComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordSectionComponentBuilder<DiscordInteractionResponseDataBuilder> WithSectionComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordSectionComponent();
        Data.Components.Add(component);
        return new DiscordSectionComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordTextDisplayComponentBuilder<DiscordInteractionResponseDataBuilder> WithTextDisplayComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordTextDisplayComponent();
        Data.Components.Add(component);
        return new DiscordTextDisplayComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordThumbnailComponentBuilder<DiscordInteractionResponseDataBuilder> WithThumbnailComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordThumbnailComponent();
        Data.Components.Add(component);
        return new DiscordThumbnailComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordMediaGalleryComponentBuilder<DiscordInteractionResponseDataBuilder> WithMediaGalleryComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordMediaGalleryComponent();
        Data.Components.Add(component);
        return new DiscordMediaGalleryComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordFileComponentBuilder<DiscordInteractionResponseDataBuilder> WithFileComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordFileComponent();
        Data.Components.Add(component);
        return new DiscordFileComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordSeparatorComponentBuilder<DiscordInteractionResponseDataBuilder> WithSeparatorComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordSeparatorComponent();
        Data.Components.Add(component);
        return new DiscordSeparatorComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }

    public DiscordContainerComponentBuilder<DiscordInteractionResponseDataBuilder> WithContainerComponent()
    {
        if (Data.Components == null)
            Data.Components = new DiscordComponentList();

        var component = new DiscordContainerComponent();
        Data.Components.Add(component);
        return new DiscordContainerComponentBuilder<DiscordInteractionResponseDataBuilder>(this, component);
    }
}