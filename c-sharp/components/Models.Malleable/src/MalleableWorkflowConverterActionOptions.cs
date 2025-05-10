using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowConverterActionOptions : MalleableWorkflowActionOptions
    {
        
        /// <summary>
        /// Name of the next step to execute if this step is successful
        /// 
        /// If this is the last step, this should be set to null
        /// </summary>
        [JsonPropertyName("nextStep")]
        public string? NextStep { get; set; }

        [JsonPropertyName("converter")]
        public MalleableConverterReference? Converter { get; set; }

        public override IEnumerable<string> GetStepNames()
        {
            var stepNames = new List<string>();
            if (!string.IsNullOrEmpty(NextStep))
            {
                stepNames.Add(NextStep);
            }
            return stepNames;
        }

        public override bool IsValid(ILogger logger)
        {
            if (Converter == null)
            {
                logger.LogError("Converter is null");
                return false;
            }
            return true;
        }
    }
}