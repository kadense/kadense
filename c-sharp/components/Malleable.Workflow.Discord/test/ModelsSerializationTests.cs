using System.Text.Json;
using Kadense.Models.Discord;
using Kadense.Models.Discord.ResponseBuilders;

namespace Kadense.Malleable.Workflow.Discord.Tests {
    public class ModelsSerializationTests
    {
        [Fact]
        public void TestSlashCommand()
        {
            string json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.Discord.Tests.Resources.slash-command-example.json");
            var slashCommand = JsonSerializer.Deserialize<DiscordInteraction>(json);
            Assert.NotNull(slashCommand);
            Assert.Equal("A_UNIQUE_TOKEN", slashCommand.Token);
            Assert.Equal("53908232506183680", slashCommand!.Member!.User!.Id);
            Assert.Equal("cardsearch", slashCommand!.Data!.Name);
        }

        [Fact]
        public void TestInteractionResponse()
        {
            var response = new DiscordInteractionResponseBuilder()
                .WithResponseType(DiscordInteractionResponseType.CHANNEL_MESSAGE_WITH_SOURCE)
                .WithData()
                    .WithTTS(false)
                    .WithContent("This is a test response")
                .End()
                .Build();

            string json = JsonSerializer.Serialize(response);
            string expected = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Malleable.Workflow.Discord.Tests.Resources.interaction-response.json");
            Assert.Equal(expected, json);
        }
    }
}