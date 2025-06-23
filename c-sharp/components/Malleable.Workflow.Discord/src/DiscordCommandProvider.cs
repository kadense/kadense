using Kadense.Malleable.Workflow.Discord.Models;

namespace Kadense.Malleable.Workflow.Discord;

public class DiscordCommandProvider : MalleableWorkflowProvider
{
    public DiscordCommandProvider()
    {
        Commands = new Dictionary<string, Func<DiscordInteraction, DiscordInteractionResponse>>();
    }
    protected Dictionary<string, Func<DiscordInteraction, DiscordInteractionResponse>> Commands { get; }

    public void RegisterCommand(string commandName, Func<DiscordInteraction, DiscordInteractionResponse> commandHandler)
    {
        if (Commands.ContainsKey(commandName))
            throw new InvalidOperationException($"Command '{commandName}' is already registered.");

        Commands[commandName] = commandHandler;
    }
    
    public DiscordInteractionResponse? ExecuteCommand(string commandName, DiscordInteraction interaction)
    {
        if (Commands.TryGetValue(commandName, out var commandHandler))
        {
            return commandHandler(interaction);
        }

        throw new InvalidOperationException($"Command '{commandName}' is not registered.");
    }

}
