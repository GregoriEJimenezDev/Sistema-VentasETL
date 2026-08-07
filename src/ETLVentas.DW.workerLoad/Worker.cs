using ETLVentas.DW.application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.workerLoad;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(
        IServiceScopeFactory scopeFactory,
        ILogger<Worker> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("=== INICIO DEL ETL COMPLETO (extracción + carga) ===");

            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider;

            // Fase 1: Extracción (BD transaccional + API + CSV) hacia staging
            _logger.LogInformation("Fase 1: Extracción de datos hacia staging...");
            var etlOrchestrator = provider.GetRequiredService<EtlOrchestratorService>();
            await etlOrchestrator.RunAsync(stoppingToken);

            // Fase 2: Carga de ventas al DWH
            _logger.LogInformation("Fase 2: Carga de ventas al DWH...");
            var csvPath = _configuration["CsvSettings:FilePath"]
                ?? throw new InvalidOperationException("CSV path not configured in 'CsvSettings:FilePath'");

            var ventasHandler = provider.GetRequiredService<VentasHandlerService>();
            await ventasHandler.ExecuteAsync(csvPath, stoppingToken);

            _logger.LogInformation("=== ETL COMPLETO FINALIZADO CORRECTAMENTE ===");
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Proceso cancelado.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error fatal que causó el cierre del Worker");
            throw;
        }

        try
        {
            _logger.LogInformation("ETL completado. Manteniendo el Worker vivo para revisión de logs. Presiona Ctrl+C para detener.");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Worker detenido por el usuario.");
        }
    }
}
