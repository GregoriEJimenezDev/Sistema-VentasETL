using Microsoft.Extensions.Logging;
using System.Globalization;
using ETLVentas.DW.domain.Interfaces;

namespace ETLVentas.DW.persistencia.Repositories;

public class VentasCsvFileReaderRepository : ICsvVentasFileReaderRepository
{
    private readonly ILogger<VentasCsvFileReaderRepository> _logger;

    public VentasCsvFileReaderRepository(ILogger<VentasCsvFileReaderRepository> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<VentaCsvRow>> ReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var records = new List<VentaCsvRow>();

        if (!File.Exists(filePath))
        {
            _logger.LogError("Archivo CSV no encontrado: {FilePath}", filePath);
            throw new FileNotFoundException($"Archivo CSV no encontrado: {filePath}");
        }

        _logger.LogInformation("Iniciando lectura de archivo CSV: {FilePath}", filePath);

        using var reader = new StreamReader(filePath);
        
        // Saltar encabezados
        var header = await reader.ReadLineAsync(cancellationToken);
        if (header == null)
        {
            _logger.LogWarning("Archivo CSV vacío o solo contiene encabezados");
            return records;
        }

        _logger.LogDebug("Encabezado detectado: {Header}", header);

        int lineNumber = 1;
        string? line;
        
        while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            lineNumber++;
            
            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var fields = line.Split(',');
                
                if (fields.Length < 8)
                {
                    _logger.LogWarning("Línea {LineNumber} tiene menos de 8 campos, se omite: {Line}", lineNumber, line);
                    continue;
                }

                var record = new VentaCsvRow(
                    Fecha: DateTime.ParseExact(fields[0].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Categoria: fields[1].Trim(),
                    Producto: fields[2].Trim(),
                    Cliente: fields[3].Trim(),
                    Suplidor: fields[4].Trim(),
                    Cantidad: int.Parse(fields[5].Trim(), CultureInfo.InvariantCulture),
                    PrecioBase: decimal.Parse(fields[6].Trim(), CultureInfo.InvariantCulture),
                    Total: decimal.Parse(fields[7].Trim(), CultureInfo.InvariantCulture)
                );

                records.Add(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parseando línea {LineNumber}: {Line}", lineNumber, line);
            }
        }

        _logger.LogInformation("Lectura completada. Registros válidos: {Count}", records.Count);
        return records;
    }
}