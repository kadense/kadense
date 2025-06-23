using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Discord.Models;

[MalleableClass("malleable", "discord", "DiscordRequestAndResponse")]
public class DiscordRequestAndResponse : MalleableBase
{
    public DiscordInteraction? Request { get; set; }
    public DiscordInteractionResponse? Response { get; set; }
}