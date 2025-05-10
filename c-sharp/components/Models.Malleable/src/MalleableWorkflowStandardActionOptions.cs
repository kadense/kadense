using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowStandardActionOptions : MalleableWorkflowActionOptions
    {
        [JsonPropertyName("nextStep")]
        public string? NextStep { get; set; }

        public override bool IsValid(ILogger logger)
        {
            if (string.IsNullOrEmpty(NextStep))
            {
                logger.LogError("NextStep is null or empty");
                return false;
            }
            return true;
        }
    }
}