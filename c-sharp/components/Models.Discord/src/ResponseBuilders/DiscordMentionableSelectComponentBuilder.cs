namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordMentionableSelectComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordMentionableSelectComponent>
    where TParent : DiscordComponentBuilder
{
    public DiscordMentionableSelectComponentBuilder(TParent parent, DiscordMentionableSelectComponent component) : base(parent, component)
    {
    }

    public DiscordMentionableSelectComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }
    public DiscordMentionableSelectComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordMentionableSelectComponentBuilder<TParent> WithMaxValues(int maxValues)
    {
        Component.MaxValues = maxValues;
        return this;
    }

    public DiscordMentionableSelectComponentBuilder<TParent> WithMinValues(int minValues)
    {
        Component.MinValues = minValues;
        return this;
    }

    public DiscordMentionableSelectComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }

    public DiscordMentionableSelectComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }
}