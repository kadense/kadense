namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordButtonComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordButtonComponent>
{
    public DiscordButtonComponentBuilder(TParent parent, DiscordButtonComponent component) : base(parent, component)
    {
    }

    public DiscordButtonComponentBuilder<TParent> WithLabel(string label)
    {
        Component.Label = label;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithStyle(int style)
    {
        Component.Style = style;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithUrl(string url)
    {
        Component.Url = url;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithEmoji(DiscordEmoji emoji)
    {
        Component.Emoji = emoji;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithSkuId(string skuId)
    {
        Component.SkuId = skuId;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithDisabled(bool disabled)
    {
        Component.Disabled = disabled;
        return this;
    }

    public DiscordButtonComponentBuilder<TParent> WithDisabled()
    {
        Component.Disabled = true;
        return this;
    }


}