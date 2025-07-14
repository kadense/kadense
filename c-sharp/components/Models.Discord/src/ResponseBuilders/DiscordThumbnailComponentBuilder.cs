namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordThumbnailComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordThumbnailComponent>
{
    public DiscordThumbnailComponentBuilder(TParent parent, DiscordThumbnailComponent component) : base(parent, component)
    {
    }

    public DiscordThumbnailComponentBuilder<TParent> WithUrl(string url)
    {
        Component.Url = url;
        return this;
    }

    public DiscordThumbnailComponentBuilder<TParent> WithMedia(DiscordUnfurledMediaItem media)
    {
        Component.Media = media;
        return this;
    }
}