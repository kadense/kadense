namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordEmbedFieldBuilder
{
    public DiscordEmbedFieldBuilder(DiscordEmbedBuilder parent, DiscordEmbedField field)
    {
        Parent = parent;
        Field = field;
    }

    protected DiscordEmbedBuilder Parent { get; }
    protected DiscordEmbedField Field { get; }

    public DiscordEmbedFieldBuilder WithName(string name)
    {
        Field.Name = name;
        return this;
    }
    public DiscordEmbedFieldBuilder WithValue(string value)
    {
        Field.Value = value;
        return this;
    }
    public DiscordEmbedFieldBuilder WithInline(bool inline)
    {
        Field.Inline = inline;
        return this;
    }
    public DiscordEmbedBuilder End()
    {
        return Parent;
    }

    public DiscordEmbedFieldBuilder WithInline()
    {
        Field.Inline = true;
        return this;
    }
}