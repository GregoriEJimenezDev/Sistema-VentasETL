using AnalisisVentas.Data.Services;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.WkService;


public class Worker : BackgroundService
{
    private readonly EtlOrchestratorService _etlOrchestrator;
    private readonly ILogger<Worker> _logger;

    public Worker(EtlOrchestratorService etlOrchestrator, ILogger<Worker> logger)
    {
        _etlOrchestrator = etlOrchestrator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _etlOrchestrator.RunAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado durante el proceso ETL");
        }
    }
}
