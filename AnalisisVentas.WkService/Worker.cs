using System.Diagnostics;
using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.WkService;

// Principio S: el Worker coordina la extracción, cada fuente la maneja su repositorio.
// Principio D: depende solo de abstracciones (interfaces), nunca de implementaciones concretas.
public class Worker : BackgroundService
{
    private readonly IFileReaderRepository<ProductoCsv> _csvProductoRepo;
    private readonly IFileReaderRepository<ClienteCsv> _csvClienteRepo;
    private readonly IApiReaderRepository<Supplier> _apiSuplidorRepo;
    private readonly IDbReaderRepository<OrderDetail> _dbVentasRepo;
    private readonly IConfiguration _config;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IFileReaderRepository<ProductoCsv> csvProductoRepo,
        IFileReaderRepository<ClienteCsv> csvClienteRepo,
        IApiReaderRepository<Supplier> apiSuplidorRepo,
        IDbReaderRepository<OrderDetail> dbVentasRepo,
        IConfiguration config,
        ILogger<Worker> logger)
    {
        _csvProductoRepo = csvProductoRepo;
        _csvClienteRepo = csvClienteRepo;
        _apiSuplidorRepo = apiSuplidorRepo;
        _dbVentasRepo = dbVentasRepo;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var totalRegistros = 0;

        _logger.LogInformation("=== INICIO DEL PROCESO ETL (Extracción) — {Timestamp} ===", DateTime.Now);

        try
        {
            var rutaProductos = _config["CsvPaths:Productos"]!;
            var productos = await _csvProductoRepo.ReadFileAsync(rutaProductos);
            totalRegistros += productos.Count();
            _logger.LogInformation("Productos CSV extraídos: {Cantidad}", productos.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo al extraer Productos CSV. Se continúa con el siguiente extractor.");
        }

        try
        {
            var rutaClientes = _config["CsvPaths:Clientes"]!;
            var clientes = await _csvClienteRepo.ReadFileAsync(rutaClientes);
            totalRegistros += clientes.Count();
            _logger.LogInformation("Clientes CSV extraídos: {Cantidad}", clientes.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo al extraer Clientes CSV. Se continúa con el siguiente extractor.");
        }

        try
        {
            var urlSuplidores = _config["ApiSettings:SuppliersUrl"]!;
            var suplidores = await _apiSuplidorRepo.ReadFromApiAsync(urlSuplidores);
            totalRegistros += suplidores.Count();
            _logger.LogInformation("Suplidores API extraídos: {Cantidad}", suplidores.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo al extraer Suplidores API. Se continúa con el siguiente extractor.");
        }

        try
        {
            var ventas = await _dbVentasRepo.ReadFromDbAsync();
            totalRegistros += ventas.Count();
            _logger.LogInformation("Ventas BD extraídas: {Cantidad}", ventas.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo al extraer Ventas BD. Se continúa con el siguiente extractor.");
        }

        stopwatch.Stop();

        _logger.LogInformation("=== FIN DEL PROCESO ETL (Extracción) — Total de registros extraídos: {Total} — Tiempo total: {Elapsed} ===",
            totalRegistros, stopwatch.Elapsed);
    }
}
