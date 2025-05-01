using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using System.Reactive.Disposables;

namespace Kadense.Testing
{
    public class KadenseTestLogger : ILogger
    {
        public readonly ITestOutputHelper Output;

        public KadenseTestLogger(ITestOutputHelper output, LogLevel minimumLevel, string categoryName)
        {
            Output = output;
            MinimumLevel = minimumLevel;
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public LogLevel MinimumLevel { get; }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return Disposable.Empty;
        }

        public bool IsEnabled(LogLevel logLevel) =>  logLevel >= MinimumLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            try
            {
                Output.WriteLine($"[{logLevel}] {CategoryName} {formatter(state, exception)}");

                if (exception != null)
                {
                    Output.WriteLine(exception.ToString());
                }
            }
            catch (Exception e)
            {
                // ignore 'There is no currently active test.'
                if (e.ToString().Contains("There is no currently active test"))
                {
                    return;
                }

                throw;
            }
        }
    }
}