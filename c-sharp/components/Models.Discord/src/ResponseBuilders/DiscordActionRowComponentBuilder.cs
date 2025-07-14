namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordActionRowComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordActionRowComponent>
{
    public DiscordActionRowComponentBuilder(TParent parent, DiscordActionRowComponent component) : base(parent, component)
    {

    }

    public DiscordButtonComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithButtonComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var button = new DiscordButtonComponent();
        Component.Components.Add(button);
        return new DiscordButtonComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, button);
    }
    
    
    public DiscordStringSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithStringSelectComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordStringSelectComponent();
        Component.Components.Add(component);
        return new DiscordStringSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }
}