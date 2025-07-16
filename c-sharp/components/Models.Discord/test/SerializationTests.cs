using System.Text.Json;

namespace Kadense.Models.Discord.Tests {
    public class SerializationTests
    {
        [Fact]
        public void TestButtonAction()
        {
            string json = KadenseTestUtils.GetEmbeddedResourceAsString("Kadense.Models.Discord.Tests.Resources.example-button-action.json");
            var interaction = JsonSerializer.Deserialize<DiscordInteraction>(json);
            Assert.NotNull(interaction);
            Assert.NotNull(interaction.Message);
            Assert.NotNull(interaction.Message.Components);
            Assert.NotEmpty(interaction.Message.Components);

            var containerComponent = interaction.Message.Components.First() as DiscordContainerComponent;
            Assert.NotNull(containerComponent);
            Assert.NotNull(containerComponent.Components);
            Assert.NotEmpty(containerComponent.Components);

            var textDisplayComponent = containerComponent.Components.First() as DiscordTextDisplayComponent;
            Assert.NotNull(textDisplayComponent);
            Assert.NotNull(textDisplayComponent.Content);
            Assert.Equal("```\n |- State: Kinda magical\n |- Location: Frontier town\n |- Big Treasure: Wagon full of garbage\n |- Guarded by: Wizard and her familiars\n```", textDisplayComponent.Content);


            var buttonComponent = interaction.Message.Components.GetByCustomId<DiscordButtonComponent>("regenerate_world");
            Assert.NotNull(buttonComponent);
            Assert.Equal("regenerate_world", buttonComponent.CustomId);

            buttonComponent = interaction.Message.Components.GetByCustomId<DiscordButtonComponent>("generate_description");
            Assert.NotNull(buttonComponent);
            Assert.Equal("generate_description", buttonComponent.CustomId);

        }
    }
}