using AnalisisVentas.Data.Class;
using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services.Extractors;


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
        var records = await factory.ReadData(_reader);

        _logger.LogInformation("CsvExtractor: extraídos {Cantidad} registros desde {FilePath}", records.Count(), _filePath);
        return records;
    }
}
