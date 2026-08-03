using AnalisisVentas.Data.Class;
using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services.Extractors;

// Principio S: este extractor solo extrae datos de archivos CSV.
// Principio D: depende de la abstracción IFileReaderRepository<T> y de ILoggerService.
public class CsvExtractor<TClass> : IExtractor<TClass> where TClass : class
{
    private readonly IFileReaderRepository<TClass> _reader;
    private readonly string _filePath;
    private readonly ILoggerService _logger;

    public CsvExtractor(IFileReaderRepository<TClass> reader, string filePath, ILoggerService logger)
    {
        _reader = reader;
        _filePath = filePath;
        _logger = logger;
    }

    public async Task<IEnumerable<TClass>> ExtractAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("CsvExtractor: iniciando extracción desde {FilePath}", _filePath);

        var factory = new FileFactory<TClass>(_filePath);
        var registros = await factory.ReadData(_reader);

        _logger.LogInformation("CsvExtractor: extraídos {Cantidad} registros desde {FilePath}", registros.Count(), _filePath);
        return registros;
    }
}
