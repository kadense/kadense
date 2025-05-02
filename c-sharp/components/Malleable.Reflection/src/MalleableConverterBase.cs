using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Kadense.Malleable.Reflection
{
    public abstract class MalleableConverterBase
    {
        protected void CompileExpression<TFrom, TTo>(string name, string expression, IDictionary<string, Delegate> expressions)
        {
            var typeTo = typeof(TTo);
            var property = typeTo.GetProperty(name)!;

            ParameterExpression[] parameterExpressions = new ParameterExpression[] {
                Expression.Parameter(typeof(TFrom), "Source")
            };
            var parameterValues = new object[] {
                Activator.CreateInstance(typeof(TFrom))!
            };
            var parsedExpression = DynamicExpressionParser.ParseLambda(parameterExpressions, property.PropertyType, expression, parameterValues);
            expressions.Add(name, parsedExpression.Compile());
        }

        protected TTo Convert<TFrom, TTo>(TFrom fromObject, IDictionary<string, Delegate> expressions)        
        {
            var toObject = Activator.CreateInstance<TTo>();

            foreach (var kvp in expressions)
            {
                var property = typeof(TTo).GetProperty(kvp.Key)!;
                object? value = kvp.Value.DynamicInvoke(new object[] { fromObject! });
                property!.SetValue(toObject, value);
            }
            return toObject;
        }
    }
}