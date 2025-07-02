using Kadense.Models.Discord;
using Kadense.Models.Malleable;

namespace Kadense.Malleable.Workflow.Discord.Tests;

public class TestDiscordCommandProvider : DiscordCommandProvider
{
    public TestDiscordCommandProvider()
    {
        RegisterCommand("cardsearch", interaction =>
        {
            return new DiscordInteractionResponse
            {
                Data = new DiscordInteractionResponseData
                {
                    Content = $"Test for '{interaction.Data?.Options!.First().Value}'",
                }
            };
        });
    }
}
