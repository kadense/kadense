namespace Kadense.Malleable.Workflow.Email;

public static class MalleableWorkflowCoordinatorFactoryExtensions
{
    public static MalleableWorkflowCoordinatorFactory AddSendEmail(this MalleableWorkflowCoordinatorFactory factory, string actionType = "SendEmail")
    {
        factory.AddAction(actionType, (ctx, stepName) =>
        {
            var step = ctx.Workflow.Spec!.Steps![stepName];
            var inputType = ctx.StepInputTypes[stepName];
            Type? outputType = null;
            if (step.Options != null)
            {
                if (step.Options.OutputType != null)
                {
                    var outputTypeName = step.Options.OutputType.GetQualifiedModuleName();
                    outputType = ctx.Assemblies[outputTypeName].Types[step.Options.OutputType.ClassName!];
                }
            }
            if (outputType == null)
                outputType = inputType;

            var processorType = typeof(SendEmailProcessor<,>).MakeGenericType(new Type[] { inputType, outputType });

            return processorType;
        });
        return factory;
    }
}
