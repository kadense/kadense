using System.Diagnostics;
using Akka.Persistence;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConversionProcessorAttribute : MalleableWorkflowProcessorAttribute
    {
        public ConversionProcessorAttribute(string stepName) : base(stepName, typeof(ConversionProcessor<,>))
        {
            // nothing to do
        }
        public override Type CreateType(MalleableWorkflowContext ctx, string stepName)
        {
            var step = ctx.Workflow.Spec!.Steps![stepName];
            var moduleName = step.ConverterOptions!.Converter!.GetQualifiedModuleName();
            var converterName = step.ConverterOptions!.Converter!.ConverterName;
            var converterType = ctx.Assemblies[moduleName].Types[converterName!];
            var converterAttribute = MalleableConverterAttribute.FromType(converterType);
            var convertFromAttribute = MalleableConvertFromAttribute.FromType(converterType);
            var convertToAttribute = MalleableConvertToAttribute.FromType(converterType);
            var convertFromType = ctx.Assemblies[convertFromAttribute.GetConvertFromModuleName()].Types[convertFromAttribute.ConvertFromClassName];
            var convertToType = ctx.Assemblies[convertToAttribute.GetConvertToModuleName()].Types[convertToAttribute.ConvertToClassName]; 
            var processorType = BaseType.MakeGenericType(new Type[] { convertFromType, convertToType }); 
            return processorType;
        }
    }

    [ConversionProcessor("Convert")]
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
            foreach (var parameter in assembly.ExpressionParameters)
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
            var destination = StepDefinition.ConverterOptions!.NextStep ?? StepDefinition.Options!.NextStep;
            var convertedObject = Converter.Convert(input);
            return (destination, convertedObject);
        }

        public override string? GetErrorDestination()
        {
            return StepDefinition.ConverterOptions!.ErrorStep;
        }
    }
}