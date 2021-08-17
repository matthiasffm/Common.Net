using System;

namespace Common;

/// <summary>
/// Loglevel
/// </summary>
public enum Severity { Debug, Information, Warning, Error, Fatal }

/// <summary>
/// Eintrag im Logger
/// </summary>
public record LogEntry(Severity Severity, string Message, Exception? Exception);

/// <summary>
/// abstrahiert das Logging in ein einfach per DI zu füllendes Interface
/// </summary>
/// <remarks>
/// einfache Abbildung auf NLog z.B. im DI Setup per
/// container.RegisterConditional(typeof(ILogger),
///     c => typeof(NlogAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
///     Lifestyle.Singleton,
///     c => true);
/// </remarks>
public interface ILogger
{
    /// <summary>
    /// Loggt <paramref name="entry"/>
    /// </summary>
    void Log(LogEntry entry);
}

/// <summary>
/// Leere Logging-Implementierung z.B. für Unittests
/// </summary>
public class NoLogger : ILogger
{
    public void Log(LogEntry entry) { }
}

/// <summary>
/// Hilfsklasse für quality of life Logging-Methoden, die es einerseits einfacher machen, Exceptions oder Informations zu loggen ohne
/// extra einen <see cref="LogEntry"/> erstellen zu müssen, trotzdem aber immer noch nur eine Methode im <see cref="ILogger"/> erfordern
/// um jegliches konkrete Logger-Framework per DI zu konfigurieren.
/// </summary>
public static class LoggerExtension
{
    /// <summary>
    /// Loggt eine Message auf dem Loglevel 'Information'.
    /// </summary>
    public static void Log(this ILogger logger, string message)
    {
        logger.Log(new LogEntry(Severity.Information, message, null));
    }

    /// <summary>
    /// Loggt eine Message auf dem angegebenen Loglevel.
    /// </summary>
    public static void Log(this ILogger logger, Severity severity, string message)
    {
        logger.Log(new LogEntry(severity, message, null));
    }

    /// <summary>
    /// Loggt eine Exception auf dem Loglevel 'Error'.
    /// </summary>
    public static void Log(this ILogger logger, Exception exception)
    {
        logger.Log(new LogEntry(Severity.Error, exception.Message, exception));
    }

    /// <summary>
    /// Loggt eine Exception mit angegebener Message auf dem Loglevel 'Error'.
    /// </summary>
    public static void Log(this ILogger logger, string message, Exception exception)
    {
        logger.Log(new LogEntry(Severity.Error, message, exception));
    }
}
