using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowIfElseExpression : IMalleableValidated, IMalleableHasStepTargets
    {
        [JsonPropertyName("expression")]
        public string? Expression { get; set; }

        [JsonPropertyName("nextStep")]
        public string? NextStep { get; set; }

        public bool IsValid(ILogger logger)
        {
            if (Expression == null)
            {
                logger.LogError("Expression is null");
                return false;
            }
            return true;
        }

        public IEnumerable<string> GetStepNames()
        {
            var stepNames = new List<string>();
            if (!string.IsNullOrEmpty(NextStep))
            {
                stepNames.Add(NextStep);
            }
            return stepNames;
        }
    }
}