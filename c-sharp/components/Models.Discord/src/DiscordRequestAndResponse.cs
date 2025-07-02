using Kadense.Malleable.Reflection;
using Kadense.Models.Malleable;

namespace Kadense.Models.Discord;

[MalleableClass("malleable", "discord", "DiscordRequestAndResponse")]
public class DiscordRequestAndResponse : MalleableBase
{
    public DiscordInteraction? Request { get; set; }
    public DiscordInteractionResponse? Response { get; set; }
}