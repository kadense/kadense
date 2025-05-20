using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Kadense.Malleable.Reflection
{
    public abstract class MalleableBase
    {
        /// <summary>
        /// The constructor for the malleable base class is included for the reflection solution
        /// </summary>
        public MalleableBase() 
        {

        }

        public static Func<T, string> CompileStringExpression<T>(string expression)
        {
            var type = typeof(T);

            ParameterExpression[] parameterExpressions = new ParameterExpression[] {
                Expression.Parameter(type, "Input")
            };
            var parameterValues = new object[] {
                Activator.CreateInstance(type)!
            };
            var parsedExpression = DynamicExpressionParser.ParseLambda(parameterExpressions, typeof(string), expression, parameterValues);
            var compiledExpression = parsedExpression.Compile();
            return (Func<T, string>)compiledExpression;
        }

        public static string GetExpressionResult<T>(T input, Func<T, string> expression)
            where T : MalleableBase
        {
            return expression(input);
        }
    }
}