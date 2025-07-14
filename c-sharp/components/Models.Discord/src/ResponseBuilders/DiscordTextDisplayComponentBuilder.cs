namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordTextDisplayComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordTextDisplayComponent>
{
    public DiscordTextDisplayComponentBuilder(TParent parent, DiscordTextDisplayComponent component) : base(parent, component)
    {
    }

    public DiscordTextDisplayComponentBuilder<TParent> WithContent(string content)
    {
        Component.Content = content;
        return this;
    }
}