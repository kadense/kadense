namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordUserSelectComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordUserSelectComponent>
    where TParent : DiscordInteractionResponseDataBuilder
{
    public DiscordUserSelectComponentBuilder(TParent parent, DiscordUserSelectComponent component) : base(parent, component)
    {
    }

    public DiscordUserSelectComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }

    public DiscordUserSelectComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordUserSelectComponentBuilder<TParent> WithMaxValues(int maxValues)
    {
        Component.MaxValues = maxValues;
        return this;
    }

    public DiscordUserSelectComponentBuilder<TParent> WithMinValues(int minValues)
    {
        Component.MinValues = minValues;
        return this;
    }

    public DiscordUserSelectComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }

    public DiscordUserSelectComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }
}