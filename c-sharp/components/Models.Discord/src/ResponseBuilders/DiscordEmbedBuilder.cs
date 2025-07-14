namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordEmbedBuilder
{
    public DiscordEmbedBuilder(DiscordInteractionResponseDataBuilder parent, DiscordEmbed embed)
    {
        Parent = parent;
        Embed = embed;
    }

    protected DiscordInteractionResponseDataBuilder Parent { get; }
    protected DiscordEmbed Embed { get; }

    public DiscordInteractionResponseDataBuilder End()
    {
        return Parent;
    }

    public DiscordEmbedBuilder WithTitle(string title)
    {
        Embed.Title = title;
        return this;
    }

    public DiscordEmbedBuilder WithDescription(string description)
    {
        Embed.Description = description;
        return this;
    }

    public DiscordEmbedBuilder WithUrl(string url)
    {
        Embed.Url = url;
        return this;
    }

    public DiscordEmbedBuilder WithColor(int color)
    {
        Embed.Color = color;
        return this;
    }

    public DiscordEmbedBuilder WithFooter(DiscordEmbedFooter footer)
    {
        Embed.Footer = footer;
        return this;
    }

    public DiscordEmbedBuilder WithImage(DiscordEmbedImage image)
    {
        Embed.Image = image;
        return this;
    }

    public DiscordEmbedBuilder WithThumbnail(DiscordEmbedImage thumbnail)
    {
        Embed.Thumbnail = thumbnail;
        return this;
    }

    public DiscordEmbedBuilder WithAuthor(DiscordEmbedAuthor author)
    {
        Embed.Author = author;
        return this;
    }

    public DiscordEmbedFieldBuilder WithField()
    {
        if (Embed.Fields == null)
            Embed.Fields = new List<DiscordEmbedField>();

        var field = new DiscordEmbedField();
        Embed.Fields.Add(field);
        return new DiscordEmbedFieldBuilder(this, field);
    }
}