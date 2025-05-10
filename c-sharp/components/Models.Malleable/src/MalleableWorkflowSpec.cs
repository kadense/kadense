using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowSpec : IMalleableValidated, IMalleableHasModules
    {
        /// <summary>
        /// The module description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Steps for this workflow
        /// </summary>
        [JsonPropertyName("steps")]
        public Dictionary<string, MalleableWorkflowStep>? Steps { get; set; }

        /// <summary>
        /// APIs for this workflow
        /// </summary>
        [JsonPropertyName("apis")]
        public Dictionary<string, MalleableWorkflowApi>? APIs { get; set; }


        private bool ValidateStepNames(string name, IEnumerable<string> stepNames, ILogger logger)
        {
            var unmatched = stepNames!.Where(x => !this.Steps!.Keys.Contains(x));
            foreach (var step in unmatched)
            {
                logger.LogError($"{name} references {step} which is not defined in the workflow");
                return false;
            }
            return true;
        }

        private bool ValidateApiNames(string name, IEnumerable<string> apiNames, ILogger logger)
        {
            var apis = this.APIs ?? new Dictionary<string, MalleableWorkflowApi>();
            var unmatched = apis.Keys.Where(x => apiNames.Contains(x));
            foreach (var api in unmatched)
            {
                logger.LogError($"Step {name} references API {api} which is not defined in the workflow");
                return false;
            }
            return true;
        }

        

        public bool IsValid(ILogger logger)
        {
            if (Steps == null || Steps.Count == 0)
            {
                logger.LogError("Workflow has no steps defined");
                return false;
            }

            foreach (var step in Steps)
            {
                if(!ValidateStepNames(step.Key, step.Value.GetStepNames(), logger))
                {
                    logger.LogError($"Step {step.Key} has invalid step names");
                    return false;
                }

                if(!step.Value.IsValid(logger))
                {
                    logger.LogError($"Step {step.Key} is invalid");
                    return false;
                }

                if(!ValidateApiNames(step.Key, step.Value.GetApiNames(), logger))
                {
                    logger.LogError($"Step {step.Key} has invalid API names");
                    return false;
                }
            }

            foreach(var api in APIs!)
            {
                if(!ValidateStepNames(api.Key, api.Value.GetStepNames(), logger))
                {
                    logger.LogError($"API {api.Key} has invalid step names");
                    return false;
                }

                if(!api.Value.IsValid(logger))
                {
                    logger.LogError($"API {api.Key} is invalid");
                    return false;
                }
            }   

            return true;
        }

        public IEnumerable<string> GetModuleNames()
        {
            var items = new List<string>();
            if (Steps != null)
            {
                foreach (var step in Steps)
                {
                    items.AddRange(step.Value.GetModuleNames());
                }
            }

            if (APIs != null)
            {
                foreach (var api in APIs)
                {
                    items.AddRange(api.Value.GetModuleNames());
                }
            }
            return items.Distinct();
        }
    }
}