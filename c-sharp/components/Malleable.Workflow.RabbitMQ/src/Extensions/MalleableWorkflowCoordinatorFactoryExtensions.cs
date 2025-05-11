using System.Text.Json;
using System.Threading.Channels;
using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Kadense.Malleable.Workflow.RabbitMQ.Extensions
{
    public static class MalleableWorkflowCoordinatorFactoryExtensions
    {
        public static MalleableWorkflowCoordinatorFactory AddRMQConnection(this MalleableWorkflowCoordinatorFactory factory, IConnection connection, string executorType = "RabbitMQ")
        {
            factory.AddExternalStepAction(executorType, (context, stepName) =>
            {
                var @namespace = context.Workflow.Metadata.NamespaceProperty;
                var name = context.Workflow.Metadata.Name;
                var step = context.Workflow.Spec!.Steps![stepName];
                var queueName = $"{@namespace}.{name}.{stepName}";
                var channel = connection.CreateChannelAsync().Result;
                var rmqContext = new RMQContext(channel, queueName, queueName, queueName);
                var rmqConnection = new MalleableWorkflowRMQConnection(rmqContext);
                var inputType = context.StepInputTypes[stepName];
                var outputType = context.StepOutputTypes[stepName];
                var processorType = factory.GetActions().Create(context, stepName)!;

                var type = typeof(MalleableWorkflowStepWithExternalQueue<,,,>).MakeGenericType(new Type[] { inputType, outputType, processorType, typeof(MalleableWorkflowRMQConnection) });
                var stepWithExternalQueue = (MalleableWorkflowStepWithExternalQueue)Activator.CreateInstance(type, new object[] { context, stepName, rmqConnection })!;
                return stepWithExternalQueue;
            });
            return factory;
        }

        public static MalleableWorkflowCoordinatorFactory AddRabbitMQWriter(this MalleableWorkflowCoordinatorFactory factory, string actionType = "RabbitMQWriter")
        {
            factory.AddAction(actionType, (ctx, stepName) =>
            {
                var step = ctx.Workflow.Spec!.Steps![stepName];
                var inputType = ctx.StepInputTypes[stepName];
                Type? outputType = null;
                if(step.Options != null)
                {
                    if(step.Options.OutputType != null)
                    {
                        var outputTypeName = step.Options.OutputType.GetQualifiedModuleName();
                        outputType = ctx.Assemblies[outputTypeName].Types[step.Options.OutputType.ClassName!];
                    }
                }
                if (outputType == null)
                    outputType = inputType;
                
                var processorType = typeof(RMQWriteProcessor<,>).MakeGenericType(new Type[] { inputType, outputType }); 
                
                return processorType;
            });
            return factory;
        }
    }
}