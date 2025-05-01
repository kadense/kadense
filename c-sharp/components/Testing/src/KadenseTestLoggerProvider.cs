using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Kadense.Testing
{
    public class KadenseTestLoggerProvider : ILoggerProvider
    {
        public KadenseTestLoggerProvider(ITestOutputHelper logger)
        {
            Logger = logger;
            MinimumLevel = LogLevel.Information;
        }

        public LogLevel MinimumLevel { get; set; }
        public ITestOutputHelper Logger { get; }

        public ILogger CreateLogger(string categoryName) => new KadenseTestLogger(Logger, MinimumLevel, categoryName);

        public void Dispose()
        {
            
        }
    }
}