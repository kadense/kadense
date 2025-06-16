using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Net.Http.Json;
using Akka.Persistence;
using Kadense.Malleable.Reflection;


namespace Kadense.Malleable.Workflow.Processing
{
    [MalleableWorkflowProcessor("WriteApi", typeof(ApiWriteProcessor<,>))]
    public class ApiWriteProcessor<TIn, TOut> : MalleableWorkflowProcessor<TIn, TOut>
        where TIn : MalleableBase
        where TOut : MalleableBase
    {
        public ApiWriteProcessor(MalleableWorkflowContext context, string stepName) : base(context, stepName)
        {
            var step = context.Workflow.Spec!.Steps![stepName];

            var path = step.Options!.Parameters["Path"];
            PathExpression = CompileExpression<string>(path);
        }

        Func<TIn, string> PathExpression { get; }




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
                Content = JsonContent.Create(message, message.GetType(), options: this.Context.GetJsonSerializerOptions())
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