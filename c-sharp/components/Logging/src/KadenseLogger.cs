using Microsoft.Extensions.Logging.Console;

namespace Kadense.Logging
{
    public interface IKadenseLogger : ILogger
    {
        KadenseLogger<TChild> Create<TChild>();
    }

    public abstract class KadenseLogger
    {
        public static KadenseLogger<T> CreateLogger<T>()
        {
            return new KadenseLogger<T>();
        }

        public static KadenseLogger<T> CreateLogger<T>(ILoggerFactory logFactory)
        {
            return new KadenseLogger<T>(logFactory);
        }

        public static KadenseLogger CreateLogger(Type loggerType)
        {
            var genericType = typeof(KadenseLogger<>).MakeGenericType(loggerType);
            return (KadenseLogger)Activator.CreateInstance(genericType)!;
        }

        public static KadenseLogger CreateLogger(Type loggerType, ILoggerFactory logFactory)
        {
            var genericType = typeof(KadenseLogger<>).MakeGenericType(loggerType);
            return (KadenseLogger)Activator.CreateInstance(genericType, new object[] { logFactory })!;
        }

        public abstract void LogInformation(string message);
        public abstract void LogInformation(string message, params object[] args);

        public abstract void LogInformation(Exception exception, string message, params object[] args);

        public abstract void LogWarning(string message);

        public abstract void LogWarning(string message, params object[] args);

        public abstract void LogWarning(Exception exception, string message, params object[] args);

        public abstract void LogError(string message);

        public abstract void LogError(string message, params object[] args);

        public abstract void LogError(Exception exception, string message, params object[] args);

        public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
    }

    public class KadenseLogger<T> : KadenseLogger, IKadenseLogger
    {
        public ILogger<T> Logger { get; }

        private ILoggerFactory LogFactory { get; set; }

        public KadenseLogger()
        {

            this.LogFactory = LoggerFactory.Create(logs =>
            {
                logs.Configure(logging =>
                {
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

        public override void LogInformation(string message)
        {
            Logger.LogInformation(message);
        }

        public override void LogInformation(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }

        public override void LogInformation(Exception exception, string message, params object[] args)
        {
            Logger.LogInformation(exception, message, args);
        }

        public override void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        public override void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        public override void LogWarning(Exception exception, string message, params object[] args)
        {
            Logger.LogWarning(exception, message, args);
        }

        public override void LogError(string message)
        {
            Logger.LogError(message);
        }

        public override void LogError(string message, params object[] args)
        {
            Logger.LogError(message, args);
        }

        public override void LogError(Exception exception, string message, params object[] args)
        {
            Logger.LogError(exception, message, args);
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
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