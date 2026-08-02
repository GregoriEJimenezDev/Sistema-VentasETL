using System.Globalization;
using AnalisisVentas.Data.Interfaces;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Csv;

// Principio S: esta clase solo lee archivos CSV.
// Principio O: abierta para extensión (implementa la interfaz), cerrada para modificación.
// Principio L: sustituible por IFileReaderRepository<T>.
// Principio D: depende de la abstracción ILogger, no de una concreción.
public class CsvVentasFileReaderRepository<TClass> : IFileReaderRepository<TClass> where TClass : class
{
    private readonly ILogger<CsvVentasFileReaderRepository<TClass>> _logger;

    public CsvVentasFileReaderRepository(ILogger<CsvVentasFileReaderRepository<TClass>> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TClass>> ReadFileAsync(string filePath)
    {
        var registros = new List<TClass>();
        try
        {
            _logger.LogInformation("Iniciando lectura del archivo CSV: {FilePath}", filePath);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await foreach (var registro in csv.GetRecordsAsync<TClass>())
            {
                registros.Add(registro);
            }

            _logger.LogInformation("Lectura del archivo CSV completada: {FilePath} — {Cantidad} registros", filePath, registros.Count);
            return registros;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer el archivo CSV: {FilePath}", filePath);
            throw;
        }
    }
}
