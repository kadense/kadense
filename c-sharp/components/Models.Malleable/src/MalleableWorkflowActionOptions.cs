using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public abstract class MalleableWorkflowActionOptions : IMalleableWorkflowActionOptions
    {

        /// <summary>
        /// Name of the next step to execute if this step fails
        ///
        /// If this is the last step, this should be set to null
        /// </summary>
        [JsonPropertyName("errorStep")]
        public string? ErrorStep { get; set; }

        [JsonPropertyName("parameters")]

        /// <summary>
        /// Parameters to pass to the action
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The name of the steps this action is linked to
        /// </summary>
        public virtual IEnumerable<string> GetStepNames()
        {
            var stepNames = new List<string>();
            if (!string.IsNullOrEmpty(ErrorStep))
            {
                stepNames.Add(ErrorStep);
            }
            return stepNames;
        }

        public abstract bool IsValid(ILogger logger);


        [JsonPropertyName("inputType")] 
        public MalleableTypeReference? InputType { get; set; }

    }
}