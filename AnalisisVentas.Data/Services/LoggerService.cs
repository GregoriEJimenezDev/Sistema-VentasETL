using System.Globalization;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Services;

// Principio S: servicio dedicado de logging (monitoreo y trazabilidad del ETL).
// Principio D: se abstrae detrás de ILoggerService para que los servicios del ETL
// no dependan directamente de Microsoft.Extensions.Logging.
public class LoggerService : ILoggerService
{
    private readonly ILogger _logger;

    public LoggerService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("AnalisisVentas.ETL");
    }

    public void LogInformation(string message, params object?[] args) => _logger.LogInformation(message, args);

    public void LogWarning(string message, params object?[] args) => _logger.LogWarning(message, args);

    public void LogError(string message, params object?[] args) => _logger.LogError(message, args);

    public void LogError(Exception exception, string message, params object?[] args) => _logger.LogError(exception, message, args);

    public void LogMetric(string metricName, TimeSpan duration, string message, params object?[] args)
    {
        var detalle = args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, message, args) : message;
        _logger.LogInformation("[METRICA: {Metric}] {Detail} — Tiempo: {Duration}", metricName, detalle, duration);
    }
}
