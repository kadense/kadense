using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowApiOptions : IMalleableValidated, IMalleableHasStepTargets
    {
        [JsonPropertyName("parameters")]
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("nextStep")]
        public string? NextStep { get; set; }

        public bool IsValid(ILogger logger)
        {
            if (Parameters == null)
            {
                logger.LogError("Parameters is null");
                return false;
            }

            if(NextStep == null)
            {
                logger.LogError("NextStep is null");
                return false;
            }

            foreach (var parameter in Parameters)
            {
                if (string.IsNullOrEmpty(parameter.Key) || string.IsNullOrEmpty(parameter.Value))
                {
                    logger.LogError($"Parameter {parameter.Key} is null or empty");
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<string> GetStepNames()
        {
            var items = new List<string>();
            if (!string.IsNullOrEmpty(NextStep))
            {
                items.Add(NextStep);
            }
            return items;
        }
    }
}