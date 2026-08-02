using System.Text.Json;
using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Api;

// Principio S: esta clase solo lee datos desde una API externa.
// Principio O: abierta para extensión, cerrada para modificación.
// Principio L: sustituible por IApiReaderRepository<Supplier>.
// Principio D: depende de abstracciones (IHttpClientFactory, IConfiguration, ILogger).
public class ApiSuplidorReaderRepository : IApiReaderRepository<Supplier>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiSuplidorReaderRepository> _logger;

    public ApiSuplidorReaderRepository(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ApiSuplidorReaderRepository> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Supplier>> ReadFromApiAsync(string url)
    {
        try
        {
            _logger.LogInformation("Iniciando consulta de suplidores a la API: {Url}", url);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var suplidores = JsonSerializer.Deserialize<List<Supplier>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Supplier>();

            _logger.LogInformation("Consulta a la API completada: {Url} — {Cantidad} suplidores", url, suplidores.Count);
            return suplidores;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar suplidores a la API: {Url}", url);
            throw;
        }
    }
}
