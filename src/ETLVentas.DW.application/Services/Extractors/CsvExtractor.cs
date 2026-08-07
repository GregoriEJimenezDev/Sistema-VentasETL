using ETLVentas.DW.domain.Interfaces;

namespace ETLVentas.DW.application.Services.Extractors;


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

        var records = await _reader.ReadFileAsync(_filePath);

        _logger.LogInformation("CsvExtractor: extraídos {Cantidad} registros desde {FilePath}", records.Count(), _filePath);
        return records;
    }
}
