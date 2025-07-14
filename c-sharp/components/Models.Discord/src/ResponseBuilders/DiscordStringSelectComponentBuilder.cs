namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordStringSelectComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordStringSelectComponent>
{
    public DiscordStringSelectComponentBuilder(TParent parent, DiscordStringSelectComponent component) : base(parent, component)
    {

    }
    public DiscordStringSelectComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }
    public DiscordStringSelectComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordStringSelectComponentBuilder<TParent> WithMaxValues(int maxValues)
    {
        Component.MaxValues = maxValues;
        return this;
    }
    public DiscordStringSelectComponentBuilder<TParent> WithMinValues(int minValues)
    {
        Component.MinValues = minValues;
        return this;
    }

    public DiscordStringSelectComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }
    public DiscordStringSelectComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }

    public DiscordStringSelectComponentBuilder<TParent> WithOptions(List<DiscordCommandOptionChoice> options)
    {
        Component.Options = options;
        return this;
    }

    public DiscordStringSelectComponentBuilder<TParent> WithOption(DiscordCommandOptionChoice option)
    {
        if (Component.Options == null)
            Component.Options = new List<DiscordCommandOptionChoice>();

        Component.Options.Add(option);
        return this;
    }
}