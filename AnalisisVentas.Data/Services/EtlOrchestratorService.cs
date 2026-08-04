using System.Diagnostics;
using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services;


public class EtlOrchestratorService
{
    private readonly IExtractor<ProductoCsv> _csvProductoExtractor;
    private readonly IExtractor<ClienteCsv> _csvClienteExtractor;
    private readonly IExtractor<Supplier> _apiSuplidorExtractor;
    private readonly IExtractor<Product> _dbProductoExtractor;
    private readonly IExtractor<Category> _dbCategoriaExtractor;
    private readonly IExtractor<Customer> _dbClienteExtractor;
    private readonly IExtractor<City> _dbCiudadExtractor;
    private readonly IExtractor<Order> _dbOrdenExtractor;
    private readonly IExtractor<OrderDetail> _dbVentasExtractor;

    private readonly IStagingService _staging;
    private readonly ILoggerService _logger;

    public EtlOrchestratorService(
        IExtractor<ProductoCsv> csvProductoExtractor,
        IExtractor<ClienteCsv> csvClienteExtractor,
        IExtractor<Supplier> apiSuplidorExtractor,
        IExtractor<Product> dbProductoExtractor,
        IExtractor<Category> dbCategoriaExtractor,
        IExtractor<Customer> dbClienteExtractor,
        IExtractor<City> dbCiudadExtractor,
        IExtractor<Order> dbOrdenExtractor,
        IExtractor<OrderDetail> dbVentasExtractor,
        IStagingService staging,
        ILoggerService logger)
    {
        _csvProductoExtractor = csvProductoExtractor;
        _csvClienteExtractor = csvClienteExtractor;
        _apiSuplidorExtractor = apiSuplidorExtractor;
        _dbProductoExtractor = dbProductoExtractor;
        _dbCategoriaExtractor = dbCategoriaExtractor;
        _dbClienteExtractor = dbClienteExtractor;
        _dbCiudadExtractor = dbCiudadExtractor;
        _dbOrdenExtractor = dbOrdenExtractor;
        _dbVentasExtractor = dbVentasExtractor;
        _staging = staging;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var stopwatchTotal = Stopwatch.StartNew();
        _logger.LogInformation("=== INICIO DEL PROCESO ETL — {Timestamp} ===", DateTime.Now);

        // Las fuentes de BD son críticas (abortan el proceso si fallan); las API/CSV son
        // tolerantes a fallos y continúan con datos vacíos. Task.WhenAll dispara todas en paralelo.
        var stopwatchExtraccion = Stopwatch.StartNew();

        var tProductos = GuardAsync(_dbProductoExtractor.ExtractAsync(cancellationToken), "Products (BD)");
        var tCategorias = GuardAsync(_dbCategoriaExtractor.ExtractAsync(cancellationToken), "Categories (BD)");
        var tClientes = GuardAsync(_dbClienteExtractor.ExtractAsync(cancellationToken), "Customers (BD)");
        var tCiudades = GuardAsync(_dbCiudadExtractor.ExtractAsync(cancellationToken), "Cities (BD)");
        var tOrdenes = GuardAsync(_dbOrdenExtractor.ExtractAsync(cancellationToken), "Orders (BD)");
        var tDetalles = GuardAsync(_dbVentasExtractor.ExtractAsync(cancellationToken), "Order_Details (BD)");
        var tSuplidores = GuardAsync(_apiSuplidorExtractor.ExtractAsync(cancellationToken), "Suplidores (API)");
        var tProductosCsv = GuardAsync(_csvProductoExtractor.ExtractAsync(cancellationToken), "Productos (CSV)");
        var tClientesCsv = GuardAsync(_csvClienteExtractor.ExtractAsync(cancellationToken), "Clientes (CSV)");

        await Task.WhenAll(tProductos, tCategorias, tClientes, tCiudades, tOrdenes, tDetalles, tSuplidores, tProductosCsv, tClientesCsv);

        var (productosOk, productos) = await tProductos;
        var (categoriasOk, categorias) = await tCategorias;
        var (clientesOk, clientes) = await tClientes;
        var (ciudadesOk, ciudades) = await tCiudades;
        var (ordenesOk, ordenes) = await tOrdenes;
        var (detallesOk, detalles) = await tDetalles;
        var (_, suplidores) = await tSuplidores;
        var (_, productosCsv) = await tProductosCsv;
        var (_, clientesCsv) = await tClientesCsv;

        if (!productosOk || !categoriasOk || !clientesOk || !ciudadesOk || !ordenesOk || !detallesOk)
        {
            _logger.LogError("Una o más fuentes de BD críticas fallaron. Se aborta el proceso ETL.");
            return;
        }

        stopwatchExtraccion.Stop();
        _logger.LogMetric("Extraccion", stopwatchExtraccion.Elapsed,
            "Productos: {0} | Categorías: {1} | Clientes: {2} | Ciudades: {3} | Órdenes: {4} | Detalles: {5} | Suplidores API: {6} | Productos CSV: {7} | Clientes CSV: {8}",
            productos.Count(), categorias.Count(), clientes.Count(), ciudades.Count(), ordenes.Count(),
            detalles.Count(), suplidores.Count(), productosCsv.Count(), clientesCsv.Count());

        // Se persisten los datos extraídos en archivos temporales (staging) como entregable de la fase de extracción.
        var stopwatchStaging = Stopwatch.StartNew();

        await _staging.WriteAsync("productos-bd", productos, cancellationToken);
        await _staging.WriteAsync("categorias-bd", categorias, cancellationToken);
        await _staging.WriteAsync("clientes-bd", clientes, cancellationToken);
        await _staging.WriteAsync("ciudades-bd", ciudades, cancellationToken);
        await _staging.WriteAsync("ordenes-bd", ordenes, cancellationToken);
        await _staging.WriteAsync("detalles-bd", detalles, cancellationToken);
        await _staging.WriteAsync("suplidores-api", suplidores, cancellationToken);
        await _staging.WriteAsync("productos-csv", productosCsv, cancellationToken);
        await _staging.WriteAsync("clientes-csv", clientesCsv, cancellationToken);

        stopwatchStaging.Stop();
        stopwatchTotal.Stop();

        _logger.LogMetric("Staging", stopwatchStaging.Elapsed, "9 conjuntos de datos persistidos");
        _logger.LogInformation("=== FIN DEL PROCESO ETL — Extracción total: {Tiempo} ===", stopwatchTotal.Elapsed);
    }

    private async Task<(bool Ok, IEnumerable<T> Data)> GuardAsync<T>(Task<IEnumerable<T>> tarea, string fuente)
    {
        try
        {
            return (true, await tarea);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo al extraer {Fuente}. Se omite.", fuente);
            return (false, Array.Empty<T>());
        }
    }
}