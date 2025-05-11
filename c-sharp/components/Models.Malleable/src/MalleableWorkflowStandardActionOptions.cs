using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowStandardActionOptions : MalleableWorkflowActionOptions
    {
        [JsonPropertyName("nextStep")]
        public string? NextStep { get; set; }

        public override bool IsValid(ILogger logger)
        {
            return true;
        }
    }
}