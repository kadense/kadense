namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordMediaGalleryComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordMediaGalleryComponent>
{
    public DiscordMediaGalleryComponentBuilder(TParent parent, DiscordMediaGalleryComponent component) : base(parent, component)
    {
    }

    public DiscordMediaGalleryComponentBuilder<TParent> WithItem(DiscordMediaGalleryItem item)
    {
        if(Component.Items == null)
            Component.Items = new List<DiscordMediaGalleryItem>();

        Component.Items.Add(item);
        return this;
    }
}