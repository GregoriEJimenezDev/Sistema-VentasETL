using System.Text.Json;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ETLVentas.DW.persistencia.Staging;


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

    public async Task WriteAsync<T>(string name, IEnumerable<T> records, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_directory);
        var filePath = Path.Combine(_directory, $"{name}.json");

        var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json, cancellationToken);

        _logger.LogInformation("Staging: {Cantidad} registros de {Nombre} escritos en {Ruta}", records.Count(), name, filePath);
    }

    public async Task<IEnumerable<T>> ReadAsync<T>(string name, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_directory, $"{name}.json");

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Staging: archivo {Nombre} no encontrado en {Ruta}. Se devuelve lista vacía.", name, filePath);
            return Array.Empty<T>();
        }

        await using var stream = File.OpenRead(filePath);
        var records = await JsonSerializer.DeserializeAsync<List<T>>(stream, cancellationToken: cancellationToken);

        _logger.LogInformation("Staging: {Cantidad} registros de {Nombre} leídos desde {Ruta}", records?.Count ?? 0, name, filePath);
        return records ?? new List<T>();
    }
}
