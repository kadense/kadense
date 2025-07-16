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
    
    public DiscordTextInputComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithTextInputComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordTextInputComponent();
        Component.Components.Add(component);
        return new DiscordTextInputComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }

    public DiscordUserSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithUserSelectComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordUserSelectComponent();
        Component.Components.Add(component);
        return new DiscordUserSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }

    public DiscordRoleSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithRoleSelectComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordRoleSelectComponent();
        Component.Components.Add(component);
        return new DiscordRoleSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }

    public DiscordMentionableSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithMentionableSelectComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordMentionableSelectComponent();
        Component.Components.Add(component);
        return new DiscordMentionableSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }

    public DiscordChannelSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>> WithChannelSelectComponent()
    {
        if (Component.Components == null)
            Component.Components = new List<DiscordComponent>();

        var component = new DiscordChannelSelectComponent();
        Component.Components.Add(component);
        return new DiscordChannelSelectComponentBuilder<DiscordActionRowComponentBuilder<TParent>>(this, component);
    }

}