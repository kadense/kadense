using System.Diagnostics;
using Akka.Persistence;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public class ConversionProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public ConversionProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var assemblyName = StepDefinition.ConverterOptions!.Converter!.GetQualifiedModuleName();
            var converterName = StepDefinition.ConverterOptions!.Converter!.ConverterName;
            var assembly = Context.Assemblies[assemblyName];
            var converterType = assembly.Types[converterName!];
            var expressionParameters = new Dictionary<string, object>();
            foreach(var parameter in assembly.ExpressionParameters)
            {
                expressionParameters.Add(parameter.Key, Activator.CreateInstance(parameter.Value)!);
            }
            Converter = (MalleableConverterBase<TIn, TOut>)Activator.CreateInstance(converterType, new object[] { expressionParameters })!;
        }

        public MalleableConverterBase<TIn, TOut> Converter { get; set; }

        public override (string?, MalleableBase) Process(MalleableBase message)
        {
            if (message is not TIn input)
            {
                throw new InvalidOperationException($"Invalid message type. Expected {typeof(TIn)}, but got {message.GetType()}.");
            }
            var destination = StepDefinition.ConverterOptions!.NextStep;
            var convertedObject = Converter.Convert(input);
            return (destination, convertedObject);
        }

        public override string? GetErrorDestination()
        {
            return StepDefinition.ConverterOptions!.ErrorStep;
        }
    }
}