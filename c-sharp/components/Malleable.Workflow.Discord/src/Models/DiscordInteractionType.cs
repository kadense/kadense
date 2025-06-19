using System.Text.Json.Serialization;

namespace Kadense.Malleable.Workflow.Discord.Models;

public enum DiscordInteractionType
{
    PING = 1,
    APPLICATION_COMMAND = 2,
    MESSAGE_COMPONENT = 3,
    APPLICATION_COMMAND_AUTOCOMPLETE = 4,
    MODAL_SUBMIT = 5
}