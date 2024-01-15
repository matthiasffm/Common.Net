namespace matthiasffm.Common;

/// <summary>
/// log level
/// </summary>
public enum Severity
{
    /// <summary>log level Debug</summary>
    Debug,

    /// <summary>log level Information</summary>
    Information,

    /// <summary>log level Warning</summary>
    Warning,

    /// <summary>log level Error</summary>
    Error,

    /// <summary>log level Fatal</summary>
    Fatal
}

/// <summary>
/// a single entry in the log
/// </summary>
public record LogEntry(Severity Severity, string Message, Exception? Exception);

/// <summary>
/// this interface abstracts the logging for use in di containers
/// </summary>
/// <example>
/// here is a simple mapping to NLog in a DI container setup
/// container.RegisterConditional(typeof(ILogger),
///     c => typeof(NlogAdapter).MakeGenericType(c.Consumer.ImplementationType),
///     Lifestyle.Singleton,
///     c => true);
/// </example>
public interface ILogger
{
    /// <summary>
    /// logs the data in <paramref name="entry"/>
    /// </summary>
    void Log(LogEntry entry);
}

/// <summary>
/// empty implementation for unit tests
/// </summary>
public class NoLogger : ILogger
{
    /// <summary>
    /// does nothing with <paramref name="entry"/>
    /// </summary>
    public void Log(LogEntry entry) { }
}

/// <summary>
/// extension methods to log a message or exception directly without creating a <see cref="LogEntry"/> object first.
/// </summary>
public static class LoggerExtension
{
    /// <summary>
    /// logs a text message on log level 'Debug'
    /// </summary>
    public static void Debug(this ILogger logger, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Log(new LogEntry(Severity.Debug, message, null));
    }

    /// <summary>
    /// logs a text message on log level 'Information'
    /// </summary>
    public static void Log(this ILogger logger, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Log(new LogEntry(Severity.Information, message, null));
    }

    /// <summary>
    /// logs a text message on the specified log level
    /// </summary>
    public static void Log(this ILogger logger, Severity severity, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Log(new LogEntry(severity, message, null));
    }

    /// <summary>
    /// logs an exception on log level 'Error'
    /// </summary>
    public static void Log(this ILogger logger, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(exception);
        logger.Log(new LogEntry(Severity.Error, exception.Message, exception));
    }

    /// <summary>
    /// logs an exception with the specified text message on log level 'Error'
    /// </summary>
    public static void Log(this ILogger logger, string message, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Log(new LogEntry(Severity.Error, message, exception));
    }
}
