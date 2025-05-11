using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Net.Http.Json;
using Akka.Persistence;
using Kadense.Malleable.Reflection;


namespace Kadense.Malleable.Workflow.Processing
{
    public class ApiWriteProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public ApiWriteProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var step = context.Workflow.Spec!.Steps![stepName];
            if(step.Action != "WriteApi")
                throw new InvalidOperationException($"Invalid action for WriteApiProcessor. Expected 'WriteApi', but got '{step.Action}'.");

            var path = step.Options!.Parameters["Path"];
            PathExpression = CompileStringExpression(path);
        }

        Func<TIn, string> PathExpression { get; } 

        public Func<TIn, string> CompileStringExpression(string expression)
        {
            var type = typeof(TIn);

            ParameterExpression[] parameterExpressions = new ParameterExpression[] {
                Expression.Parameter(type, "Input")
            };
            var parameterValues = new object[] {
                Activator.CreateInstance(type)!
            };
            var parsedExpression = DynamicExpressionParser.ParseLambda(parameterExpressions, typeof(string), expression, parameterValues);
            var compiledExpression = parsedExpression.Compile();
            return (Func<TIn, string>)compiledExpression;
        }


        public override (string?, MalleableBase) Process(MalleableBase message)
        {
            if (message is not TIn input)
            {
                throw new InvalidOperationException($"Invalid message type. Expected {typeof(TIn)}, but got {message.GetType()}.");
            }
            var baseUrl = StepDefinition.Options!.Parameters["baseUrl"];
            var path = PathExpression.Invoke(input);
            var destination = StepDefinition.Options!.NextStep;
            var task = ProcessApiWrite(input, baseUrl, path);
            task.Wait();
            var convertedObject = task.Result;
            return (destination, convertedObject);
        }

        public async Task<TOut> ProcessApiWrite(TIn message, string baseUrl, string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/{path}")
            {
                Content = JsonContent.Create(message, message.GetType())
            });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<TOut>();
            if (content == null)
            {
                throw new InvalidOperationException("Failed to deserialize response content.");
            }
            return content;
        }

        public override string? GetErrorDestination()
        {
            return StepDefinition.Options!.ErrorStep;
        }
    }
}