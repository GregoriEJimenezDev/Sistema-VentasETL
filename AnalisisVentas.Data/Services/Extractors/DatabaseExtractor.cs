using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services.Extractors;


public class DatabaseExtractor<TClass> : IExtractor<TClass> where TClass : class
{
    private readonly IDbReaderRepository<TClass> _reader;
    private readonly ILoggerService _logger;
    private readonly string _source;

    public DatabaseExtractor(IDbReaderRepository<TClass> reader, ILoggerService logger)
    {
        _reader = reader;
        _logger = logger;
        _source = typeof(TClass).Name;
    }

    public async Task<IEnumerable<TClass>> ExtractAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("DatabaseExtractor: iniciando extracción de {Source}", _source);

        var records = await _reader.ReadFromDbAsync();

        _logger.LogInformation("DatabaseExtractor: extraídos {Cantidad} registros de {Source}", records.Count(), _source);
        return records;
    }
}
