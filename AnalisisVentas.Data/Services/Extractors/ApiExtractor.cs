using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services.Extractors;

// Principio S: este extractor solo consume una API REST mediante HttpClient.
// Principio D: depende de la abstracción IApiReaderRepository<T> y de ILoggerService.
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

        var registros = await _reader.ReadFromApiAsync(_url);

        _logger.LogInformation("ApiExtractor: extraídos {Cantidad} registros desde {Url}", registros.Count(), _url);
        return registros;
    }
}
