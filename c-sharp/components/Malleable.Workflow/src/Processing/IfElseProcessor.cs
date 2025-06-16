using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using Akka.Persistence;
using Kadense.Malleable.Reflection;


namespace Kadense.Malleable.Workflow.Processing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IfElseProcessorAttribute : MalleableWorkflowProcessorAttribute
    {
        public IfElseProcessorAttribute(string stepName) : base(stepName, typeof(IfElseProcessor<>))
        {
            // nothing to do
        }

        public override Type CreateType(MalleableWorkflowContext ctx, string stepName)
        {
            var inputType = ctx.StepInputTypes[stepName];
                
            var processorType = BaseType.MakeGenericType(new Type[] { inputType }); 
            
            return processorType;
        }
    }

    [IfElseProcessor("IfElse")]
    public class IfElseProcessor<T> : MalleableWorkflowProcessor<T, T>
        where T : MalleableBase
    {
        public IfElseProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var step = context.Workflow.Spec!.Steps![stepName];

            ifElseExpressions = new List<Func<T, bool>>();
            foreach (var condition in step.IfElseOptions!.Expressions)
            {
                var expression = condition.Expression;
                if (string.IsNullOrWhiteSpace(expression))
                    throw new InvalidOperationException($"Expression cannot be null or empty. Expression: {expression}");

                var compiledExpression = CompileExpression<bool>(expression);
                ifElseExpressions.Add(compiledExpression);
            }
        }

        List<Func<T, bool>> ifElseExpressions { get; }



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