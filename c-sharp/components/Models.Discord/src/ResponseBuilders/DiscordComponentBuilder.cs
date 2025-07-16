namespace Kadense.Models.Discord.ResponseBuilders;

public abstract class DiscordComponentBuilder
{

} 

public class DiscordComponentBuilder<TParent, T> : DiscordComponentBuilder
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