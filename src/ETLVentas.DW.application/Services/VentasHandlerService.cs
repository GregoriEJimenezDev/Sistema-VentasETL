using System.Globalization;
using ETLVentas.DW.domain.Entities.Api;
using ETLVentas.DW.domain.Entities.Csv;
using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Entities.Dimensions;
using ETLVentas.DW.domain.Entities.Facts;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.application.Services;

public class VentasHandlerService
{
    private readonly IStagingService _staging;
    private readonly ISalesDwhRepository _dwhRepository;
    private readonly ILogger<VentasHandlerService> _logger;

    public VentasHandlerService(
        IStagingService staging,
        ISalesDwhRepository dwhRepository,
        ILogger<VentasHandlerService> logger)
    {
        _staging = staging;
        _dwhRepository = dwhRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("=== INICIANDO FASE 2: CARGA AL DWH DESDE STAGING ===");

        // 1. Leer los 9 conjuntos de staging generados por la Fase 1 (Extracción).
        //    La Fase 2 consume EXCLUSIVAMENTE los archivos que escribió la Fase 1.
        _logger.LogInformation("Paso 1: Leyendo staging de la Fase 1...");
        var productosBd = (await _staging.ReadAsync<Product>("productos-bd", cancellationToken)).ToList();
        var categoriasBd = (await _staging.ReadAsync<Category>("categorias-bd", cancellationToken)).ToList();
        var clientesBd = (await _staging.ReadAsync<Customer>("clientes-bd", cancellationToken)).ToList();
        var ciudadesBd = (await _staging.ReadAsync<City>("ciudades-bd", cancellationToken)).ToList();
        var ordenesBd = (await _staging.ReadAsync<Order>("ordenes-bd", cancellationToken)).ToList();
        var detallesBd = (await _staging.ReadAsync<OrderDetail>("detalles-bd", cancellationToken)).ToList();
        var suplidoresApi = (await _staging.ReadAsync<Supplier>("suplidores-api", cancellationToken)).ToList();
        var productosCsv = (await _staging.ReadAsync<ProductoCsv>("productos-csv", cancellationToken)).ToList();
        var clientesCsv = (await _staging.ReadAsync<ClienteCsv>("clientes-csv", cancellationToken)).ToList();

        _logger.LogInformation("Staging leído -> Productos BD: {ProductosBD} | Categorías: {Categorias} | Clientes BD: {ClientesBD} | Ciudades: {Ciudades} | Órdenes: {Ordenes} | Detalles: {Detalles} | Suplidores API: {Suplidores} | Productos CSV: {ProductosCSV} | Clientes CSV: {ClientesCSV}",
            productosBd.Count, categoriasBd.Count, clientesBd.Count, ciudadesBd.Count,
            ordenesBd.Count, detallesBd.Count, suplidoresApi.Count, productosCsv.Count, clientesCsv.Count);

        if (detallesBd.Count == 0)
        {
            _logger.LogWarning("No hay detalles de ventas en staging. Finalizando Fase 2 sin cargas.");
            return;
        }

        // 2. Transformar y deduplicar dimensiones a partir de los datos REALES del staging.
        //    La BD transaccional es la fuente de verdad para el modelo dimensional; los
        //    staging de CSV (productos-csv/clientes-csv) son el feed plano legacy, se leen
        //    para trazabilidad pero no alimentan las dimensiones del DWH.
        _logger.LogInformation("Paso 2: Transformando y deduplicando dimensiones...");

        var categoriaPorId = categoriasBd.ToDictionary(c => c.CategoryID, c => c.CategoryName);
        var ciudadPorId = ciudadesBd.ToDictionary(c => c.CityID, c => c.CityName);

        var categorias = categoriasBd
            .Select(c => new DimCategoria
            {
                NombreCategoria = c.CategoryName,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Categorías únicas: {Count}", categorias.Count);

        var productos = productosBd
            .Select(p => new DimProducto
            {
                Codigo = p.ProductID.ToString(),
                NombreProducto = p.ProductName,
                Categoria = categoriaPorId.TryGetValue(p.CategoryID, out var categoria) ? categoria : string.Empty,
                Precio = p.Price,
                Stock = p.Stock,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Productos únicos: {Count}", productos.Count);

        var clientes = clientesBd
            .Select(c => new DimCliente
            {
                ClienteIdOrigen = c.CustomerID.ToString(),
                NombreCompleto = $"{c.FirstName} {c.LastName}".Trim(),
                Email = c.Email,
                Telefono = c.Phone,
                Ciudad = ciudadPorId.TryGetValue(c.CityID, out var ciudad) ? ciudad : string.Empty,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Clientes únicos: {Count}", clientes.Count);

        // Suplidores: provienen de la API. Si la API no estaba corriendo en la Fase 1,
        // la lista queda vacía y DimSuplidor no se puebla (el proceso no debe fallar).
        var suplidores = suplidoresApi
            .Select(s => new DimSuplidor
            {
                SuplidorIdOrigen = s.Id.ToString(),
                NombreSuplidor = $"{s.Name.Firstname} {s.Name.Lastname}".Trim(),
                Email = s.Email,
                Telefono = s.Phone,
                Ciudad = s.Address.City,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Suplidores únicos: {Count}", suplidores.Count);
        if (suplidores.Count == 0)
            _logger.LogWarning("DimSuplidor quedará vacío: la API no devolvió suplidores en esta ejecución.");

        // 3. Fechas únicas (yyyyMMdd) a partir de las fechas de las órdenes reales.
        _logger.LogInformation("Paso 3: Generando dimensión de tiempo...");
        var fechas = ordenesBd
            .Select(o => o.OrderDate.Date)
            .Distinct()
            .Select(fecha => new DimFecha
            {
                FechaKey = int.Parse(fecha.ToString("yyyyMMdd")),
                Fecha = fecha,
                Anio = fecha.Year,
                Mes = fecha.Month,
                Dia = fecha.Day,
                NombreMes = fecha.ToString("MMMM", new CultureInfo("es-ES")),
                Trimestre = (fecha.Month - 1) / 3 + 1,
                Semana = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                DiaNombre = fecha.ToString("dddd", new CultureInfo("es-ES")),
                EsFinSemana = fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Fechas únicas: {Count}", fechas.Count);

        // 4. Cargar dimensiones primero.
        _logger.LogInformation("Paso 4: Cargando dimensiones en DWH...");
        await _dwhRepository.LoadDataAsync(
            categorias, productos, clientes, suplidores, fechas, Enumerable.Empty<FactVentas>(),
            cancellationToken);
        _logger.LogInformation("Paso 4 completado. Dimensiones guardadas.");

        // 5. Resolver FKs y construir hechos desde los detalles de orden REALES.
        //    Relación: OrderDetail.OrderID -> Order (CustomerID + OrderDate); OrderDetail.ProductID -> Producto.
        _logger.LogInformation("Paso 5: Resolviendo claves foráneas y construyendo hechos...");
        var productoKeys = await _dwhRepository.GetProductoKeysAsync(productos.Select(p => p.Codigo), cancellationToken);
        var clienteKeys = await _dwhRepository.GetClienteKeysAsync(clientes.Select(c => c.ClienteIdOrigen), cancellationToken);

        var ordenPorId = ordenesBd.ToDictionary(o => o.OrderID, o => o);
        var hechos = detallesBd
            .Where(d => ordenPorId.ContainsKey(d.OrderID))
            .Select(d =>
            {
                var orden = ordenPorId[d.OrderID];
                return new FactVentas
                {
                    ProductoKey = productoKeys[d.ProductID.ToString()],
                    ClienteKey = clienteKeys[orden.CustomerID.ToString()],
                    FechaKey = int.Parse(orden.OrderDate.ToString("yyyyMMdd")),
                    Cantidad = d.Quantity,
                    PrecioUnitario = d.UnitPrice,
                    TotalVenta = d.TotalPrice
                };
            })
            .ToList();
        _logger.LogInformation("  - Hechos construidos: {Count}", hechos.Count);

        // 6. Guardar hechos.
        _logger.LogInformation("Paso 6: Guardando hechos en DWH...");
        await _dwhRepository.LoadDataAsync(
            Enumerable.Empty<DimCategoria>(),
            Enumerable.Empty<DimProducto>(),
            Enumerable.Empty<DimCliente>(),
            Enumerable.Empty<DimSuplidor>(),
            Enumerable.Empty<DimFecha>(),
            hechos,
            cancellationToken);

        _logger.LogInformation("=== FASE 2 COMPLETADA EXITOSAMENTE ===");
        _logger.LogInformation("Resumen: {Categorias} categorías, {Productos} productos, {Clientes} clientes, {Suplidores} suplidores, {Fechas} fechas, {Hechos} hechos",
            categorias.Count, productos.Count, clientes.Count, suplidores.Count, fechas.Count, hechos.Count);
    }
}
