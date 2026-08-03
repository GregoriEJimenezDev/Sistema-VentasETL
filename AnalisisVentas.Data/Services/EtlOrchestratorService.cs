using System.Collections.Concurrent;
using System.Diagnostics;
using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Entities.Dwh.Facts;
using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Services;


public class EtlOrchestratorService
{
    private const int MaxGradoDeParalelismo = 4;

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

    private readonly IDbWriterRepository<DimProducto> _dimProductoWriter;
    private readonly IDbWriterRepository<DimCliente> _dimClienteWriter;
    private readonly IDbWriterRepository<DimSuplidor> _dimSuplidorWriter;
    private readonly IDbWriterRepository<FactVentas> _factVentasWriter;

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
        IDbWriterRepository<DimProducto> dimProductoWriter,
        IDbWriterRepository<DimCliente> dimClienteWriter,
        IDbWriterRepository<DimSuplidor> dimSuplidorWriter,
        IDbWriterRepository<FactVentas> factVentasWriter,
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
        _dimProductoWriter = dimProductoWriter;
        _dimClienteWriter = dimClienteWriter;
        _dimSuplidorWriter = dimSuplidorWriter;
        _factVentasWriter = factVentasWriter;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var stopwatchTotal = Stopwatch.StartNew();
        _logger.LogInformation("=== INICIO DEL PROCESO ETL — {Timestamp} ===", DateTime.Now);

        // ================= EXTRACCIÓN EN PARALELO (E) =================
        // Atributo de calidad — rendimiento: las 9 fuentes se extraen en paralelo con
        // Task.WhenAll. Las fuentes de BD son críticas (abortan el proceso si fallan);
        // las API/CSV son tolerantes a fallos y continúan con datos vacíos.
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

        // ================= STAGING (archivos temporales JSON) =================
        // Requisito "Guardar los datos extraídos en archivos temporales o tablas staging".
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
        _logger.LogMetric("Staging", stopwatchStaging.Elapsed, "9 conjuntos de datos persistidos");

        // ================= TRANSFORMACIÓN + CARGA (T+L) =================
        var stopwatchCarga = Stopwatch.StartNew();

        // 10. Productos → DimProducto (carga en paralelo). Cada Codigo es único, por lo
        // que el UPSERT (SELECT-then-INSERT) es seguro bajo concurrencia.
        var mapaProductos = new ConcurrentDictionary<int, int>();
        var mapaCategorias = categorias.ToDictionary(c => c.CategoryID);

        await Parallel.ForEachAsync(productos, new ParallelOptions { MaxDegreeOfParallelism = MaxGradoDeParalelismo, CancellationToken = cancellationToken },
            async (producto, _) =>
            {
                try
                {
                    if (!mapaCategorias.TryGetValue(producto.CategoryID, out var categoria))
                    {
                        _logger.LogWarning("Producto {ProductID} sin categoría {CategoryID}, se omite", producto.ProductID, producto.CategoryID);
                        return;
                    }

                    var dimProducto = TransformService.MapProductoToDim(producto, categoria);
                    var productoKey = await _dimProductoWriter.UpsertAsync(dimProducto);
                    mapaProductos[producto.ProductID] = productoKey;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo al cargar producto {ProductID}. Se continúa.", producto.ProductID);
                }
            });
        _logger.LogInformation("Dimensión productos cargada: {Cantidad}", mapaProductos.Count);

        // 11. Clientes → DimCliente (carga en paralelo). Cada ClienteIdOrigen es único.
        var mapaClientes = new ConcurrentDictionary<int, int>();
        var mapaCiudades = ciudades.ToDictionary(c => c.CityID);

        await Parallel.ForEachAsync(clientes, new ParallelOptions { MaxDegreeOfParallelism = MaxGradoDeParalelismo, CancellationToken = cancellationToken },
            async (cliente, _) =>
            {
                try
                {
                    var ciudad = mapaCiudades.TryGetValue(cliente.CityID, out var city) ? city.CityName : string.Empty;
                    var dimCliente = TransformService.MapClienteToDim(cliente, ciudad);
                    var clienteKey = await _dimClienteWriter.UpsertAsync(dimCliente);
                    mapaClientes[cliente.CustomerID] = clienteKey;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo al cargar cliente {CustomerID}. Se continúa.", cliente.CustomerID);
                }
            });
        _logger.LogInformation("Dimensión clientes cargada: {Cantidad}", mapaClientes.Count);

        // 12. Suplidores API → DimSuplidor (carga en paralelo). Cada SuplidorIdOrigen es único.
        var suplidoresCargados = 0;

        await Parallel.ForEachAsync(suplidores, new ParallelOptions { MaxDegreeOfParallelism = MaxGradoDeParalelismo, CancellationToken = cancellationToken },
            async (suplidor, _) =>
            {
                try
                {
                    var dimSuplidor = TransformService.MapSuplidorToDim(suplidor);
                    await _dimSuplidorWriter.UpsertAsync(dimSuplidor);
                    Interlocked.Increment(ref suplidoresCargados);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo al cargar suplidor {Id}. Se continúa.", suplidor.Id);
                }
            });
        _logger.LogInformation("Dimensión suplidores cargada: {Cantidad}", suplidoresCargados);

        // 13. Órdenes + Detalles → FactVentas (secuencial): se preserva la idempotencia
        // del anti-duplicado SELECT-then-INSERT del repositorio y se evita que dos hilos
        // inserten el mismo hecho simultáneamente. El volumen es bajo (pocos detalles).
        var hechosInsertados = 0;

        foreach (var orden in ordenes)
        {
            if (!mapaClientes.TryGetValue(orden.CustomerID, out var clienteKey))
            {
                _logger.LogWarning("Orden {OrderID} sin cliente cargado {CustomerID}, se omite", orden.OrderID, orden.CustomerID);
                continue;
            }

            var fechaKey = int.Parse(orden.OrderDate.ToString("yyyyMMdd"));

            foreach (var detalle in detalles.Where(d => d.OrderID == orden.OrderID))
            {
                try
                {
                    if (!mapaProductos.TryGetValue(detalle.ProductID, out var productoKey))
                    {
                        _logger.LogWarning("Detalle {DetailID} sin producto cargado {ProductID}, se omite", detalle.DetailID, detalle.ProductID);
                        continue;
                    }

                    var factVentas = TransformService.MapToFactVentas(orden, detalle, productoKey, clienteKey);
                    var ventaKey = await _factVentasWriter.UpsertAsync(factVentas);

                    // El writer retorna 0 si la FechaKey no existe en DimFecha.
                    if (ventaKey == 0)
                    {
                        _logger.LogWarning("FechaKey {FechaKey} no existe en DimFecha — se omite hecho de la orden {OrderID}", fechaKey, orden.OrderID);
                        continue;
                    }

                    hechosInsertados++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo al cargar hecho de la orden {OrderID}, detalle {DetailID}. Se continúa.", orden.OrderID, detalle.DetailID);
                }
            }
        }
        _logger.LogInformation("Hechos procesados: {Insertados}", hechosInsertados);

        stopwatchCarga.Stop();
        stopwatchTotal.Stop();

        _logger.LogMetric("Carga", stopwatchCarga.Elapsed,
            "Productos: {0} | Clientes: {1} | Suplidores: {2} | Hechos: {3}",
            mapaProductos.Count, mapaClientes.Count, suplidoresCargados, hechosInsertados);
        _logger.LogInformation("=== FIN DEL PROCESO ETL — Tiempo total: {Tiempo} ===", stopwatchTotal.Elapsed);
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
