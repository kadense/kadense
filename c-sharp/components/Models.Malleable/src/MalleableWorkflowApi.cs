using Microsoft.Extensions.Logging;

namespace Kadense.Models.Malleable
{
    public class MalleableWorkflowApi : IMalleableValidated, IMalleableHasStepTargets, IMalleableHasModules
    {
        /// <summary>
        /// When type is ingress, these are the options for the ingress API
        /// </summary>
        [JsonPropertyName("ingressOptions")]
        public MalleableWorkflowApiOptions? IngressOptions { get; set; }

        /// <summary>
        /// The type of API that this action is using
        /// </summary>
        [JsonPropertyName("apiType")]
        public string? ApiType { get; set; } = "Ingress";

        /// <summary>
        /// The type of API that this action is using
        /// </summary>
        [JsonPropertyName("underlyingType")] 
        public MalleableTypeReference? UnderlyingType { get; set; }

        public IEnumerable<string> GetModuleNames()
        {
            var items = new List<string>();
            if (UnderlyingType != null)
            {
                items.Add(UnderlyingType!.GetQualifiedModuleName());
            }
            return items;
        }

        public IEnumerable<string> GetStepNames()
        {
            var items = new List<string>();
            if(IngressOptions != null)
            {
                items.AddRange(IngressOptions.GetStepNames());
            }
            return items;
        }
        

        public bool IsValid(ILogger logger)
        {
            if (ApiType == null)
            {
                logger.LogError("ApiType is null");
                return false;
            }

            if(UnderlyingType == null)
            {
                logger.LogError("UnderlyingType is null");
                return false;
            }

            switch(ApiType)
            {
                case "Ingress":
                    if (IngressOptions == null)
                    {
                        logger.LogError("IngressOptions is null");
                        return false;
                    }
                    if (!IngressOptions.IsValid(logger))
                    {
                        logger.LogError("IngressOptions is invalid");
                        return false;
                    }
                    break;
                default:
                    logger.LogError($"Unknown ApiType {ApiType}");
                    return false;
            }


            return true;
        }


        public bool ValidateInputOutputTypes(MalleableWorkflow workflow, Type? previousType, IDictionary<string, Type> availableTypes, IDictionary<string, Type> processedSteps, ILogger logger)
        {

            if(UnderlyingType == null)
            {
                logger.LogError("UnderlyingType is null");
                return false;
            }

            switch(ApiType)
            {
                case "Ingress":
                    if (IngressOptions == null)
                    {
                        logger.LogError("IngressOptions is null");
                        return false;
                    }
                    if (!IngressOptions.IsValid(logger))
                    {
                        logger.LogError("IngressOptions is invalid");
                        return false;
                    }
                    var stepName = IngressOptions.NextStep!;
                    if (stepName == null)
                    {
                        logger.LogError("NextStep is null");
                        return false;
                    }

                    if(processedSteps.ContainsKey(stepName))
                        return true;

                    var step = workflow.Spec!.Steps![stepName];
                    var className = UnderlyingType.GetQualifiedClassName();
                    var type = availableTypes[className];
                    
                    return ValidateInputOutputTypes(workflow, type, availableTypes, processedSteps, logger);

                default:
                    logger.LogError($"Unknown ApiType {ApiType}");
                    return false;
            }
        }
    }
}