namespace AnalisisVentas.Data.Interfaces;

// Principio D: abstracción de logging para que los servicios del ETL no dependan
// directamente de Microsoft.Extensions.Logging. Facilita el monitoreo y la trazabilidad.
public interface ILoggerService
{
    void LogInformation(string message, params object?[] args);
    void LogWarning(string message, params object?[] args);
    void LogError(string message, params object?[] args);
    void LogError(Exception exception, string message, params object?[] args);

    // Registra una métrica de rendimiento con su duración (requisito de validación de
    // atributos de calidad: rendimiento).
    void LogMetric(string metricName, TimeSpan duration, string message, params object?[] args);
}
