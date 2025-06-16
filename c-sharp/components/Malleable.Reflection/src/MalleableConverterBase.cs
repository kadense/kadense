using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using IdentityModel.Client;

namespace Kadense.Malleable.Reflection
{
    public abstract class MalleableConverterBase
    {
        public MalleableConverterBase(IDictionary<string, object> parameters)
        {
            Parameters = parameters;
        }

        public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        
    }
    public abstract class MalleableConverterBase<TFrom, TTo> : MalleableConverterBase
        where TFrom : MalleableBase
        where TTo : MalleableBase
    {
        public MalleableConverterBase(IDictionary<string, object> parameters) : base(parameters)
        {

        }

        protected void CompileExpression(string name, string expression, IDictionary<string, Delegate> expressions)
        {
            var typeTo = typeof(TTo);
            var property = typeTo.GetProperty(name)!;

            var customTypes = new List<Type>() { };
            foreach (var parameter in Parameters)
            {
                customTypes.Add(parameter.Value.GetType());
            }


            var cfg = new ParsingConfig();
            var customTypeProvider = new MalleableDynamicLinqCustomTypeProviders(cfg, customTypes);
            cfg.CustomTypeProvider = customTypeProvider;

            List<ParameterExpression> parameterExpressions = new List<ParameterExpression>() {
                Expression.Parameter(typeof(TFrom), "Source")
            };
            foreach (var parameter in Parameters)
            {
                var parameterExpression = Expression.Parameter(parameter.Value.GetType(), parameter.Key);
                parameterExpressions.Add(parameterExpression);
            }
            var parameterValues = new List<object> {
                Activator.CreateInstance(typeof(TFrom))!
            };


            foreach (var parameter in Parameters)
            {
                parameterValues.Add(parameter.Value);
            }

            var parsedExpression = DynamicExpressionParser.ParseLambda(cfg, parameterExpressions.ToArray(), property.PropertyType, expression, parameterValues.ToArray());
            expressions.Add(name, parsedExpression.Compile());
        }

        protected TTo Convert(TFrom fromObject, IDictionary<string, Delegate> expressions)
        {
            var toObject = Activator.CreateInstance<TTo>();

            foreach (var kvp in expressions)
            {
                var property = typeof(TTo).GetProperty(kvp.Key)!;
                List<object> parameterValues = new List<object>();
                parameterValues.Add(fromObject!);
                foreach (var parameter in Parameters)
                {
                    parameterValues.Add(parameter.Value);
                }

                object? value = kvp.Value.DynamicInvoke(parameterValues.ToArray());
                property!.SetValue(toObject, value);
            }
            return toObject;
        }

        public abstract TTo Convert(TFrom fromObject);
    }
}