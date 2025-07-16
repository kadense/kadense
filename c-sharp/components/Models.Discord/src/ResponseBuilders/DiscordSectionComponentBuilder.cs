namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordSectionComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordSectionComponent>
{
    public DiscordSectionComponentBuilder(TParent parent, DiscordSectionComponent component) : base(parent, component)
    {
    }

    public DiscordTextDisplayComponentBuilder<DiscordSectionComponentBuilder<TParent>> WithTextDisplayComponent()
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var textDisplay = new DiscordTextDisplayComponent();
        Component.Components.Add(textDisplay);
        return new DiscordTextDisplayComponentBuilder<DiscordSectionComponentBuilder<TParent>>(this, textDisplay);
    }

    public DiscordSectionComponentBuilder<TParent> WithTextDisplayComponent(string content)
    {
        if (Component.Components == null)
            Component.Components = new DiscordComponentList();

        var textDisplay = new DiscordTextDisplayComponent();
        textDisplay.Content = content;
        Component.Components.Add(textDisplay);
        return this;
    }
}