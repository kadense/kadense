namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordSeparatorComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordSeparatorComponent>
{
    public DiscordSeparatorComponentBuilder(TParent parent, DiscordSeparatorComponent component) : base(parent, component)
    {
    }

    public DiscordSeparatorComponentBuilder<TParent> WithoutDivider(bool divider = false)
    {
        Component.Divider = divider;
        return this;
    }

    public DiscordSeparatorComponentBuilder<TParent> WithSpacing(int spacing)
    {
        Component.Spacing = spacing;
        return this;
    }
}