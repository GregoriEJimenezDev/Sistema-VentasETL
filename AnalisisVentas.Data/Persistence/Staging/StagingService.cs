using System.Text.Json;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AnalisisVentas.Data.Persistence.Staging;

// Principio S: esta clase solo persiste los datos extraídos en archivos temporales
// (staging), cumpliendo el requisito "Guardar los datos extraídos en archivos
// temporales o tablas staging" de la fase de extracción.
// Principio D: depende de abstracciones (IConfiguration, ILoggerService).
public class StagingService : IStagingService
{
    private readonly string _directory;
    private readonly ILoggerService _logger;

    public StagingService(IConfiguration configuration, ILoggerService logger)
    {
        // Ruta centralizada en appsettings.json, nunca hardcodeada.
        _directory = configuration.GetSection("Staging:Directory").Value
            ?? Path.Combine(Path.GetTempPath(), "VentasETL_Staging");
        _logger = logger;
    }

    public async Task WriteAsync<T>(string nombre, IEnumerable<T> registros, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_directory);
        var filePath = Path.Combine(_directory, $"{nombre}.json");

        var json = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json, cancellationToken);

        _logger.LogInformation("Staging: {Cantidad} registros de {Nombre} escritos en {Ruta}", registros.Count(), nombre, filePath);
    }
}
