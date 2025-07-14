namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordFileComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordFileComponent>
{
    public DiscordFileComponentBuilder(TParent parent, DiscordFileComponent component) : base(parent, component)
    {
    }

    public DiscordFileComponentBuilder<TParent> WithFile(DiscordUnfurledMediaItem file)
    {
        Component.File = file;
        return this;
    }

    public DiscordFileComponentBuilder<TParent> WithSize(int size)
    {
        Component.Size = size;
        return this;
    }

    public DiscordFileComponentBuilder<TParent> WithName(string name)
    {
        Component.Name = name;
        return this;
    }
    
    public DiscordFileComponentBuilder<TParent> WithSpoiler(bool spoiler)
    {
        Component.Spoiler = spoiler;
        return this;
    }
}