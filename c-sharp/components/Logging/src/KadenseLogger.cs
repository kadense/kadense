using Microsoft.Extensions.Logging.Console;

namespace Kadense.Logging
{
    public interface IKadenseLogger : ILogger
    {
        KadenseLogger<TChild> Create<TChild>();
    }

    public class KadenseLogger<T> : IKadenseLogger
    {
        public ILogger<T> Logger { get; }

        private ILoggerFactory LogFactory { get; set; }
        
        public KadenseLogger()
        {
            
            this.LogFactory = LoggerFactory.Create(logs => {
                logs.Configure(logging => {
                    logging.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId;
                })
                .AddSimpleConsole(cfg =>
                {
                    cfg.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                    cfg.SingleLine = true;
                    cfg.UseUtcTimestamp = true;
                });
            });
            this.Logger = this.LogFactory.CreateLogger<T>();
        }

        public KadenseLogger(ILoggerFactory logFactory)
        {
            this.LogFactory = logFactory;
            this.Logger = this.LogFactory.CreateLogger<T>();
        }

        public KadenseLogger<TChild> Create<TChild>()
        {
            return new KadenseLogger<TChild>(this.LogFactory);
        }

        public void LogInformation(string message)
        {
            Logger.LogInformation(message);
        }

        public void LogInformation(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Logger.LogInformation(exception, message, args);
        }

        public void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        public void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            Logger.LogWarning(exception, message, args);
        }

        public void LogError(string message)
        {
            Logger.LogError(message);
        }

        public void LogError(string message, params object[] args)
        {
            Logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            Logger.LogError(exception, message, args);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(logLevel);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return Logger.BeginScope(state);
        }
    }
}