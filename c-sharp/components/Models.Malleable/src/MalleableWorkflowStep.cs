using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowStep : IMalleableValidated, IMalleableHasModules
    {
        /// <summary>
        /// The type of the executor to use for this step, defaults to Akka.Net
        /// </summary>
        [JsonPropertyName("executorType")]
        public string ExecutorType { get; set; } = "Akka.Net";

        /// <summary>
        /// The action to perform, defaults to Convert
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = "Convert";

        /// <summary>
        /// Input Type Reference
        /// </summary>
        [JsonPropertyName("inputType")]
        public MalleableTypeReference? InputType { get; set; }

        /// <summary>
        /// Output Type Reference
        /// </summary>
        [JsonPropertyName("outputType")]
        public MalleableTypeReference? OutputType { get; set; }

        /// <summary>
        /// Converter Type Reference
        /// </summary>
        [JsonPropertyName("converterOptions")]
        public MalleableWorkflowConverterActionOptions? ConverterOptions { get; set; }

        /// <summary>
        /// Converter Type Reference
        /// </summary>
        [JsonPropertyName("ifElseOptions")]
        public MalleableWorkflowIfElseActionOptions? IfElseOptions { get; set; }

        /// <summary>
        /// Write API Options
        /// </summary>
        [JsonPropertyName("options")]
        public MalleableWorkflowStandardActionOptions? Options { get; set; }

        public IEnumerable<string> GetStepNames()
        {
            switch (Action)
            {
                case "Convert":
                    return ConverterOptions!.GetStepNames();

                default:
                    return Enumerable.Empty<string>();
            }
        }

        public bool IsValid(ILogger logger)
        {
            switch (Action)
            {
                case "Convert":
                    if (ConverterOptions == null)
                    {
                        logger.LogError("ConverterOptions is null");
                        return false;
                    }
                    if (!ConverterOptions.IsValid(logger))
                    {
                        logger.LogError("ConverterOptions is invalid");
                        return false;
                    }
                    return true;

                case "IfElse":
                    if (IfElseOptions == null)
                    {
                        logger.LogError("IfElseOptions is null");
                        return false;
                    }
                    if (!IfElseOptions.IsValid(logger))
                    {
                        logger.LogError("IfElseOptions is invalid");
                        return false;
                    }
                    return true;

                case "WriteApi":
                    if (Options == null)
                    {
                        logger.LogError("WriteApiOptions is null");
                        return false;
                    }
                    if (!Options.IsValid(logger))
                    {
                        logger.LogError("WriteApiOptions is invalid");
                        return false;
                    }
                    return true;

                default:
                    logger.LogError($"Unknown action type: {Action}");
                    return false;
            }
        }

        public IEnumerable<string> GetApiNames()
        {
            List<string> apiNames = new List<string>();
            switch (Action)
            {
                case "WriteApi":
                    if(this.Options!.Parameters!.TryGetValue("apiName", out var apiName))
                    {
                        apiNames.Add(apiName);
                    }
                    break;
                default:
                    // do nothing
                    break;
            }
            return apiNames;
        }

        public IEnumerable<string> GetModuleNames()
        {
            var items = new List<string>();
            if (InputType != null)
            {
                items.Add(InputType.GetQualifiedModuleName());
            }
            if (OutputType != null)
            {
                items.Add(OutputType.GetQualifiedModuleName());
            }
            if (ConverterOptions != null)
            {
                if(ConverterOptions.Converter != null)
                {
                    items.Add(ConverterOptions.Converter.GetQualifiedModuleName());
                }
            }
            return items;
        }
    }
}