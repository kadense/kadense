using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowIfElseActionOptions : MalleableWorkflowActionOptions
    {
        [JsonPropertyName("expressions")]
        public List<MalleableWorkflowIfElseExpression> Expressions { get; set; } = new List<MalleableWorkflowIfElseExpression>();

        [JsonPropertyName("elseNextStep")]
        public string? ElseNextStep { get; set; }

        public override IEnumerable<string> GetStepNames()
        {
            var stepNames = new List<string>();
            if (!string.IsNullOrEmpty(ElseNextStep))
            {
                stepNames.Add(ElseNextStep);
            }
            foreach (var expression in Expressions)
            {
                stepNames.AddRange(expression.GetStepNames());
            }
            return stepNames;
        }


        public override bool IsValid(ILogger logger)
        {
            if (Expressions == null)
            {
                logger.LogError("Expressions is null");
                return false;
            }
            if (Expressions.Count == 0)
            {
                logger.LogError("Expressions is empty");
                return false;
            }
            return true;
        }
    }
}