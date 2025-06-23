using Kadense.Malleable.Workflow.Discord.Models;
using Kadense.Malleable.Workflow.Processing;

namespace Kadense.Malleable.Workflow.Discord;

[MalleableWorkflowProcessor("DiscordCommandProcessor", typeof(DiscordCommandProcessor<,>))]
public class DiscordCommandProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
    where TIn : MalleableBase
    where TOut : MalleableBase
{
    public DiscordCommandProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
    {
        string? providerName = null;
        if (StepDefinition.Options != null && StepDefinition.Options.Parameters != null)
        {
            StepDefinition.Options.Parameters.TryGetValue("provider", out providerName);
        }

        if (!Context.Providers.TryGetValue(providerName ?? "DiscordCommandProvider", out var provider))
            throw new InvalidOperationException($"DiscordCommandProvider '{providerName ?? "DiscordCommandProvider"}' not found in the context.");

        CommandProvider = (DiscordCommandProvider)provider;
    }

    public DiscordCommandProvider CommandProvider { get; }

    public override string? GetErrorDestination()
    {
        return StepDefinition.Options!.ErrorStep;
    }

    public override (string?, MalleableBase) Process(MalleableBase message)
    {
        if (message is not DiscordInteraction interaction)
            throw new InvalidOperationException($"Invalid message type. Expected {typeof(DiscordInteraction)}, but got {message.GetType()}.");

        string commandName = interaction.Data!.Name!;
        var response = CommandProvider.ExecuteCommand(commandName, interaction);
        string? nextStep = StepDefinition.Options!.NextStep;

        var requestAndResponse = new DiscordRequestAndResponse
        {
            Request = interaction,
            Response = response
        };

        return (nextStep, requestAndResponse);
    }
}