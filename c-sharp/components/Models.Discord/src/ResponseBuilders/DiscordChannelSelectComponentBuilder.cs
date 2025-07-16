namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordChannelSelectComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordChannelSelectComponent>
    where TParent : DiscordComponentBuilder
{
    public DiscordChannelSelectComponentBuilder(TParent parent, DiscordChannelSelectComponent component) : base(parent, component)
    {
    }

    public DiscordChannelSelectComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordChannelSelectComponentBuilder<TParent> WithMaxValues(int maxValues)
    {
        Component.MaxValues = maxValues;
        return this;
    }
    public DiscordChannelSelectComponentBuilder<TParent> WithMinValues(int minValues)
    {
        Component.MinValues = minValues;
        return this;
    }
    public DiscordChannelSelectComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }

    public DiscordChannelSelectComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }
    public DiscordChannelSelectComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }
    public DiscordChannelSelectComponentBuilder<TParent> WithChannelTypes(List<int> channelTypes)
    {
        Component.ChannelTypes = channelTypes;
        return this;
    }
    public DiscordChannelSelectComponentBuilder<TParent> WithChannelType(int channelType)
    {
        if (Component.ChannelTypes == null)
            Component.ChannelTypes = new List<int>();
        Component.ChannelTypes.Add(channelType);
        return this;
    }
}