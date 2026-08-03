using System.Text.Json;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AnalisisVentas.Data.Persistence.Staging;


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
