using ETLVentas.DW.domain.Interfaces;

namespace ETLVentas.DW.application.Services.Extractors;


public class ApiExtractor<TClass> : IExtractor<TClass> where TClass : class
{
    private readonly IApiReaderRepository<TClass> _reader;
    private readonly string _url;
    private readonly ILoggerService _logger;

    public ApiExtractor(IApiReaderRepository<TClass> reader, string url, ILoggerService logger)
    {
        _reader = reader;
        _url = url;
        _logger = logger;
    }

    public async Task<IEnumerable<TClass>> ExtractAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ApiExtractor: iniciando extracción desde {Url}", _url);

        var records = await _reader.ReadFromApiAsync(_url);

        _logger.LogInformation("ApiExtractor: extraídos {Cantidad} registros desde {Url}", records.Count(), _url);
        return records;
    }
}
