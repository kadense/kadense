namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordRoleSelectComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordRoleSelectComponent>
    where TParent : DiscordComponentBuilder
{
    public DiscordRoleSelectComponentBuilder(TParent parent, DiscordRoleSelectComponent component) : base(parent, component)
    {
    }
    public DiscordRoleSelectComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }
    public DiscordRoleSelectComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordRoleSelectComponentBuilder<TParent> WithMaxValues(int maxValues)
    {
        Component.MaxValues = maxValues;
        return this;
    }
    public DiscordRoleSelectComponentBuilder<TParent> WithMinValues(int minValues)
    {
        Component.MinValues = minValues;
        return this;
    }

    public DiscordRoleSelectComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }

    public DiscordRoleSelectComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }
}