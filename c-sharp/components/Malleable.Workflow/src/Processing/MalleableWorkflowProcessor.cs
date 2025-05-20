using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Kadense.Malleable.Reflection;

namespace Kadense.Malleable.Workflow.Processing
{
    public abstract class MalleableWorkflowProcessor
    {
        public abstract (string?, MalleableBase) Process(MalleableBase message);

        public string ProcessorSignature { get; set; } = string.Empty;
    }
    public abstract class MalleableWorkflowProcessor<TIn, TOut> : MalleableWorkflowProcessor
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public MalleableWorkflowProcessor(MalleableWorkflowContext context, string stepName)
        {
            Context = context;
            StepName = stepName;
            StepDefinition = context.Workflow.Spec!.Steps![stepName];
        }

        public MalleableWorkflowContext Context { get; set; }

        public string StepName { get; set; } = string.Empty;
        protected MalleableWorkflowStep StepDefinition { get; set; }

        public abstract string? GetErrorDestination();

        protected virtual Func<TIn, T> CompileExpression<T>(string expression)
        {
            var type = typeof(TIn);
            
            var parameterExpressions = new List<ParameterExpression>() {
                Expression.Parameter(type, "Input")
            };
            var parameterValues = new List<object>() {
                Activator.CreateInstance(type)!
            };

            var customTypes = new List<Type>() { };
            foreach(var assembly in Context.Assemblies.Values)
            {
                foreach(var customType in assembly.Types)
                {
                    customTypes.Add(customType.Value);
                }
                foreach(var parameter in assembly.ExpressionParameters)
                {
                    if(!customTypes.Contains(parameter.Value))
                    {
                        customTypes.Add(parameter.Value);
                        Expression.Parameter(parameter.Value, parameter.Key);
                        parameterValues.Add(Activator.CreateInstance(parameter.Value)!);
                    }
                }
            }

            
            var cfg = new ParsingConfig();
            var customTypeProvider = new MalleableDynamicLinqCustomTypeProviders(cfg, customTypes);
            cfg.CustomTypeProvider = customTypeProvider;

            var parsedExpression = DynamicExpressionParser.ParseLambda(cfg, parameterExpressions.ToArray(), typeof(T), expression, parameterValues.ToArray());
            var compiledExpression = parsedExpression.Compile();
            return (Func<TIn, T>)compiledExpression;
        }
    }
}