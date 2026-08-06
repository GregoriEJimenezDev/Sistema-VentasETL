using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AnalisisVentas.Data.Application.Services;

namespace AnalisisVentas.WkService;

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
            var csvPath = _configuration["CsvSettings:FilePath"]
                ?? throw new InvalidOperationException("CSV path not configured in 'CsvSettings:FilePath'");

            _logger.LogInformation("Worker iniciado. Procesando archivo: {Path}", csvPath);

            using var scope = _scopeFactory.CreateScope();
            var ventasHandler = scope.ServiceProvider.GetRequiredService<VentasHandlerService>();
            
            await ventasHandler.ExecuteAsync(csvPath, stoppingToken);

            _logger.LogInformation("Proceso ETL finalizado correctamente.");
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Proceso cancelado.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fatal en el proceso ETL");
            throw;
        }
    }
}