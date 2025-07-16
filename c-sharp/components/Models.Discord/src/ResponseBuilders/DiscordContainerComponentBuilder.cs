namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordContainerComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordContainerComponent>
{
    public DiscordContainerComponentBuilder(TParent parent, DiscordContainerComponent component) : base(parent, component)
    {
    }

    public DiscordSectionComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithSectionComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var section = new DiscordSectionComponent();
        Component.Components.Add(section);
        return new DiscordSectionComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, section);
    }


    public DiscordActionRowComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithActionRowComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var actionRow = new DiscordActionRowComponent();
        Component.Components.Add(actionRow);
        return new DiscordActionRowComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, actionRow);
    }

    public DiscordTextDisplayComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithTextDisplayComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var textDisplay = new DiscordTextDisplayComponent();
        Component.Components.Add(textDisplay);
        return new DiscordTextDisplayComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, textDisplay);
    }

    public DiscordMediaGalleryComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithMediaGalleryComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var mediaGallery = new DiscordMediaGalleryComponent();
        Component.Components.Add(mediaGallery);
        return new DiscordMediaGalleryComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, mediaGallery);
    }

    public DiscordSeparatorComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithSeparatorComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var separator = new DiscordSeparatorComponent();
        Component.Components.Add(separator);
        return new DiscordSeparatorComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, separator);
    }

    public DiscordFileComponentBuilder<DiscordContainerComponentBuilder<TParent>> WithFileComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var file = new DiscordFileComponent();
        Component.Components.Add(file);
        return new DiscordFileComponentBuilder<DiscordContainerComponentBuilder<TParent>>(this, file);
    }


    public DiscordContainerComponentBuilder<TParent> WithAccentColor(int accentColor)
    {
        Component.AccentColor = accentColor;
        return this;
    }

    public DiscordContainerComponentBuilder<TParent> WithSpoiler(bool spoiler = true)
    {
        Component.Spoiler = spoiler;
        return this;
    }
}