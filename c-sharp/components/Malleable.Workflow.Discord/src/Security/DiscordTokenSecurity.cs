namespace Kadense.Malleable.Workflow.Discord.Security;

public class DiscordTokenSecurity
{
    public string? Token { get; set; }

    public string? Timestamp { get; set; }

    public bool IsValid()
    {
        return true; // Placeholder for actual validation logic
    }    
}
