namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordInteractionResponseBuilder
{
    protected DiscordInteractionResponse Response { get; } = new DiscordInteractionResponse();

    public DiscordInteractionResponseBuilder WithResponseType(DiscordInteractionResponseType responseType)
    {
        Response.Type = (int)responseType;
        return this;
    }

    public DiscordInteractionResponseDataBuilder WithData()
    {
        Response.Data = new DiscordInteractionResponseData();
        var builder = new DiscordInteractionResponseDataBuilder(this, Response.Data);
        return builder;
    }

    public DiscordInteractionResponse Build()
    {
        return Response;
    }
}