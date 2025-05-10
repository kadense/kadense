using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using Akka.Persistence;
using Kadense.Malleable.Reflection;


namespace Kadense.Malleable.Workflow.Processing
{
    public class IfElseProcessor<T> : MalleableWorkflowProcessor<T, T>
        where T : MalleableBase
    {
        public IfElseProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var step = context.Workflow.Spec!.Steps![stepName];
            if(step.Action != "IfElse")
                throw new InvalidOperationException($"Invalid action for IfElseProcessor. Expected 'IfElse', but got '{step.Action}'.");

            ifElseExpressions = new List<Func<T, bool>>();
            foreach(var condition in step.IfElseOptions!.Expressions)
            {
                var expression = condition.Expression;
                if (string.IsNullOrWhiteSpace(expression))
                    throw new InvalidOperationException($"Expression cannot be null or empty. Expression: {expression}");

                var compiledExpression = CompileStringExpression(expression);
                ifElseExpressions.Add(compiledExpression);
            }
        }

        List<Func<T, bool>> ifElseExpressions { get; } 

        public Func<T, bool> CompileStringExpression(string expression)
        {
            var type = typeof(T);

            ParameterExpression[] parameterExpressions = new ParameterExpression[] {
                Expression.Parameter(type, "Input")
            };
            var parameterValues = new object[] {
                Activator.CreateInstance(type)!
            };
            var parsedExpression = DynamicExpressionParser.ParseLambda(parameterExpressions, typeof(bool), expression, parameterValues);
            var compiledExpression = parsedExpression.Compile();
            return (Func<T, bool>)compiledExpression;
        }


        public override (string?, MalleableBase) Process(MalleableBase message)
        {
            if (message is not T input)
            {
                throw new InvalidOperationException($"Invalid message type. Expected {typeof(T)}, but got {message.GetType()}.");
            }
            for (int i = 0; i < ifElseExpressions.Count; i++)
            {
                var expression = ifElseExpressions[i];
                var result = expression.Invoke(input);
                if (result)
                {
                    var destination = StepDefinition.IfElseOptions!.Expressions[i].NextStep;
                    return (destination, input);
                }
            }
            var elseDestination = StepDefinition.IfElseOptions!.ElseNextStep;
            return (elseDestination, input);
        }

        public override string? GetErrorDestination()
        {
            return StepDefinition.ConverterOptions!.ErrorStep;
        }
    }
}