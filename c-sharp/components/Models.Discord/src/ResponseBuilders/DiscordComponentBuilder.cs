namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordComponentBuilder<TParent,T>
    where T : DiscordComponent
{
    public DiscordComponentBuilder(TParent parent, T component)
    {
        Parent = parent;
        Component = component;
    }

    protected TParent Parent { get; }
    protected T Component { get; }

    public TParent End()
    {
        return Parent;
    }
}