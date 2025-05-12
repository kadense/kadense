using Kadense.Logging;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowQueueProcessor 
    {
        private static readonly KadenseLogger<MalleableWorkflowQueueProcessor> logger = new KadenseLogger<MalleableWorkflowQueueProcessor>();
        
        public static bool OnReceive<TIn, TOut, TProcessor>(object message, MalleableWorkflowContext workflowContext, TProcessor processor, string stepName)
            where TIn : MalleableBase
            where TOut : MalleableBase
            where TProcessor : MalleableWorkflowProcessor<TIn, TOut>
        {
            
            if (message is MalleableEnvelope<TIn> input)
            {
                try 
                {
                    (string? destination, MalleableBase result) = processor.Process(input.Message!);
                    if(result is not TOut processedResult)
                        throw new InvalidOperationException($"Invalid result type. Expected {typeof(TOut)}, but got {result.GetType()}.");

                    var processedEnvelope = new MalleableEnvelope<TOut>(processedResult, input.LineageId, stepName);

                    if (workflowContext.EnableMessageSigning)
                    {
                        processedEnvelope.GenerateSignatures(input.LineageSignature, processor.ProcessorSignature);
                        processor.ProcessorSignature = processedEnvelope.ProcessSignature!;
                    }

                    logger.LogInformation($"{processedEnvelope.Step} {destination ?? "{abandoned}"} {processedEnvelope.LineageId} {processedEnvelope.RawSignature} {processedEnvelope.LineageSignature} {processedEnvelope.ProcessSignature} {processedEnvelope.CombinedSignature}");
                    
                    if(!String.IsNullOrEmpty(destination))
                    {
                        workflowContext.Send(destination, processedEnvelope);
                    }

                }
                catch(Exception ex)
                {
                    var errorDestination = processor.GetErrorDestination();

                    if (errorDestination == null && !workflowContext.DebugMode)
                        throw;

                    var errorMessage = new MalleableWorkflowError<TIn>(input.Message!, ex);
                    var errorEnvelope = new MalleableEnvelope<MalleableWorkflowError<TIn>>(errorMessage, input.LineageId, stepName);
                    
                    if (workflowContext.EnableMessageSigning)
                    {
                        errorEnvelope.GenerateSignatures(input.LineageSignature, processor.ProcessorSignature);
                        processor.ProcessorSignature = errorEnvelope.ProcessSignature!;
                    }

                    logger.LogInformation($"{errorEnvelope.Step} {errorDestination ?? "{abandoned}"} {errorEnvelope.LineageId} {errorEnvelope.RawSignature} {errorEnvelope.LineageSignature} {errorEnvelope.ProcessSignature} {errorEnvelope.CombinedSignature}");
                    if (errorDestination != null)
                    {
                        workflowContext.Send(errorDestination, errorEnvelope);
                    }
                    
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}