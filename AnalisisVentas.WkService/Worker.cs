using System.Diagnostics;
using AnalisisVentas.Data.Class;
using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Entities.Dwh.Facts;
using AnalisisVentas.Data.Interfaces;
using AnalisisVentas.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.WkService;

// Principio S: el Worker solo orquesta el pipeline ETL; cada fase la ejecuta su
// repositorio/servicio especializado.
// Principio D: depende solo de abstracciones (interfaces), nunca de implementaciones concretas.
public class Worker : BackgroundService
{
    private readonly IFileReaderRepository<ProductoCsv> _csvProductoRepo;
    private readonly IFileReaderRepository<ClienteCsv> _csvClienteRepo;
    private readonly IApiReaderRepository<Supplier> _apiSuplidorRepo;
    private readonly IDbReaderRepository<OrderDetail> _dbVentasRepo;
    private readonly IDbReaderRepository<Product> _dbProductoRepo;
    private readonly IDbReaderRepository<Category> _dbCategoriaRepo;
    private readonly IDbReaderRepository<Customer> _dbClienteRepo;
    private readonly IDbReaderRepository<City> _dbCiudadRepo;
    private readonly IDbReaderRepository<Order> _dbOrdenRepo;
    private readonly IDbWriterRepository<DimProducto> _dimProductoWriter;
    private readonly IDbWriterRepository<DimCliente> _dimClienteWriter;
    private readonly IDbWriterRepository<DimSuplidor> _dimSuplidorWriter;
    private readonly IDbWriterRepository<FactVentas> _factVentasWriter;
    private readonly IConfiguration _config;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IFileReaderRepository<ProductoCsv> csvProductoRepo,
        IFileReaderRepository<ClienteCsv> csvClienteRepo,
        IApiReaderRepository<Supplier> apiSuplidorRepo,
        IDbReaderRepository<OrderDetail> dbVentasRepo,
        IDbReaderRepository<Product> dbProductoRepo,
        IDbReaderRepository<Category> dbCategoriaRepo,
        IDbReaderRepository<Customer> dbClienteRepo,
        IDbReaderRepository<City> dbCiudadRepo,
        IDbReaderRepository<Order> dbOrdenRepo,
        IDbWriterRepository<DimProducto> dimProductoWriter,
        IDbWriterRepository<DimCliente> dimClienteWriter,
        IDbWriterRepository<DimSuplidor> dimSuplidorWriter,
        IDbWriterRepository<FactVentas> factVentasWriter,
        IConfiguration config,
        ILogger<Worker> logger)
    {
        _csvProductoRepo = csvProductoRepo;
        _csvClienteRepo = csvClienteRepo;
        _apiSuplidorRepo = apiSuplidorRepo;
        _dbVentasRepo = dbVentasRepo;
        _dbProductoRepo = dbProductoRepo;
        _dbCategoriaRepo = dbCategoriaRepo;
        _dbClienteRepo = dbClienteRepo;
        _dbCiudadRepo = dbCiudadRepo;
        _dbOrdenRepo = dbOrdenRepo;
        _dimProductoWriter = dimProductoWriter;
        _dimClienteWriter = dimClienteWriter;
        _dimSuplidorWriter = dimSuplidorWriter;
        _factVentasWriter = factVentasWriter;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("=== INICIO DEL PROCESO ETL — {Timestamp} ===", DateTime.Now);

        // ================= EXTRACCIÓN (E) =================
        IEnumerable<Product> productos;
        IEnumerable<Category> categorias;
        IEnumerable<Customer> clientes;
        IEnumerable<City> ciudades;
        IEnumerable<Order> ordenes;
        IEnumerable<OrderDetail> detalles;
        IEnumerable<Supplier> suplidores;
        IEnumerable<ProductoCsv> productosCsv;
        IEnumerable<ClienteCsv> clientesCsv;

        try { productos = await _dbProductoRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Products BD"); return; }
        try { categorias = await _dbCategoriaRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Categories BD"); return; }
        try { clientes = await _dbClienteRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Customers BD"); return; }
        try { ciudades = await _dbCiudadRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Cities BD"); return; }
        try { ordenes = await _dbOrdenRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Orders BD"); return; }
        try { detalles = await _dbVentasRepo.ReadFromDbAsync(); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Order_Details BD"); return; }
        try { suplidores = await _apiSuplidorRepo.ReadFromApiAsync(_config["ApiSettings:SuppliersUrl"]!); } catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Suplidores API"); suplidores = Array.Empty<Supplier>(); }
        try
        {
            var factoryProductos = new FileFactory<ProductoCsv>(_config["CsvPaths:Productos"]!);
            productosCsv = await factoryProductos.ReadData(_csvProductoRepo);
        }
        catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Productos CSV"); productosCsv = Array.Empty<ProductoCsv>(); }
        try
        {
            var factoryClientes = new FileFactory<ClienteCsv>(_config["CsvPaths:Clientes"]!);
            clientesCsv = await factoryClientes.ReadData(_csvClienteRepo);
        }
        catch (Exception ex) { _logger.LogError(ex, "Fallo al extraer Clientes CSV"); clientesCsv = Array.Empty<ClienteCsv>(); }

        _logger.LogInformation(
            "Extracción completada — Productos BD: {P} | Categorías: {Cat} | Clientes BD: {Cli} | Ciudades: {Ci} | Órdenes: {Or} | Detalles: {Det} | Suplidores API: {Sup} | Productos CSV: {Pc} | Clientes CSV: {Cc}",
            productos.Count(), categorias.Count(), clientes.Count(), ciudades.Count(), ordenes.Count(), detalles.Count(), suplidores.Count(), productosCsv.Count(), clientesCsv.Count());

        // ================= TRANSFORMACIÓN + CARGA (T+L) =================

        // 10. Productos → DimProducto
        var mapaProductos = new Dictionary<int, int>();
        var mapaCategorias = categorias.ToDictionary(c => c.CategoryID);
        foreach (var producto in productos)
        {
            try
            {
                if (!mapaCategorias.TryGetValue(producto.CategoryID, out var categoria))
                {
                    _logger.LogWarning("Producto {ProductID} sin categoría {CategoryID}, se omite", producto.ProductID, producto.CategoryID);
                    continue;
                }

                var dimProducto = TransformService.MapProductoToDim(producto, categoria);
                var productoKey = await _dimProductoWriter.UpsertAsync(dimProducto);
                mapaProductos[producto.ProductID] = productoKey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al cargar producto {ProductID}. Se continúa.", producto.ProductID);
            }
        }
        _logger.LogInformation("Dimensión productos cargada: {Cantidad}", mapaProductos.Count);

        // 11. Clientes → DimCliente
        var mapaClientes = new Dictionary<int, int>();
        var mapaCiudades = ciudades.ToDictionary(c => c.CityID);
        foreach (var cliente in clientes)
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
        }
        _logger.LogInformation("Dimensión clientes cargada: {Cantidad}", mapaClientes.Count);

        // 12. Suplidores API → DimSuplidor
        var suplidoresCargados = 0;
        foreach (var suplidor in suplidores)
        {
            try
            {
                var dimSuplidor = TransformService.MapSuplidorToDim(suplidor);
                await _dimSuplidorWriter.UpsertAsync(dimSuplidor);
                suplidoresCargados++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al cargar suplidor {Id}. Se continúa.", suplidor.Id);
            }
        }
        _logger.LogInformation("Dimensión suplidores cargada: {Cantidad}", suplidoresCargados);

        // 13. Órdenes + Detalles → FactVentas
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

                    // Detectar si fue insert o ya existía: el writer no distingue, se cuenta como procesado.
                    hechosInsertados++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo al cargar hecho de la orden {OrderID}, detalle {DetailID}. Se continúa.", orden.OrderID, detalle.DetailID);
                }
            }
        }
        _logger.LogInformation("Hechos procesados: {Insertados}", hechosInsertados);

        stopwatch.Stop();

        _logger.LogInformation(
            "=== FIN DEL PROCESO ETL — Resumen: {Productos} productos | {Clientes} clientes | {Suplidores} suplidores | {Hechos} hechos | Tiempo total: {Tiempo} ===",
            mapaProductos.Count, mapaClientes.Count, suplidoresCargados, hechosInsertados, stopwatch.Elapsed);
    }
}
