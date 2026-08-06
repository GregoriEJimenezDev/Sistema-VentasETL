using System.Globalization;
using Microsoft.Extensions.Logging;
using AnalisisVentas.Data.Domain.Entities.Dimensions;
using AnalisisVentas.Data.Domain.Entities.Facts;
using AnalisisVentas.Data.Domain.Interfaces;

namespace AnalisisVentas.Data.Application.Services;

public class VentasHandlerService
{
    private readonly ICsvVentasFileReaderRepository _csvReader;
    private readonly ISalesDwhRepository _dwhRepository;
    private readonly ILogger<VentasHandlerService> _logger;

    public VentasHandlerService(
        ICsvVentasFileReaderRepository csvReader,
        ISalesDwhRepository dwhRepository,
        ILogger<VentasHandlerService> logger)
    {
        _csvReader = csvReader;
        _dwhRepository = dwhRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(string csvFilePath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("=== INICIANDO PROCESO ETL ===");
        _logger.LogInformation("Archivo origen: {FilePath}", csvFilePath);

        // 1. Leer CSV
        _logger.LogInformation("Paso 1: Leyendo archivo CSV...");
        var rows = await _csvReader.ReadAsync(csvFilePath, cancellationToken);
        var rowsList = rows.ToList();
        _logger.LogInformation("Paso 1 completado. Registros leídos: {Count}", rowsList.Count);

        if (rowsList.Count == 0)
        {
            _logger.LogWarning("No hay datos para procesar. Finalizando.");
            return;
        }

        // 2. Transformar y deduplicar dimensiones con LINQ
        _logger.LogInformation("Paso 2: Transformando y deduplicando dimensiones...");

        var categorias = rowsList
            .Select(r => r.Categoria)
            .Distinct()
            .Select(n => new DimCategoria
            {
                NombreCategoria = n,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Categorías únicas: {Count}", categorias.Count);

        var productos = rowsList
            .GroupBy(r => r.Producto)
            .Select(g => g.First())
            .Select(r => new DimProducto
            {
                Codigo = r.Producto, // Usamos el nombre como código ya que no hay código separado
                NombreProducto = r.Producto,
                Categoria = r.Categoria,
                Precio = r.PrecioBase,
                Stock = 0, // No disponible en CSV
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Productos únicos: {Count}", productos.Count);

        var clientes = rowsList
            .GroupBy(r => r.Cliente)
            .Select(g => g.First())
            .Select(r => new DimCliente
            {
                ClienteIdOrigen = r.Cliente,
                NombreCompleto = r.Cliente,
                Email = string.Empty,
                Telefono = string.Empty,
                Ciudad = string.Empty,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Clientes únicos: {Count}", clientes.Count);

        var suplidores = rowsList
            .GroupBy(r => r.Suplidor)
            .Select(g => g.First())
            .Select(r => new DimSuplidor
            {
                SuplidorIdOrigen = r.Suplidor,
                NombreSuplidor = r.Suplidor,
                Email = string.Empty,
                Telefono = string.Empty,
                Ciudad = string.Empty,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();
        _logger.LogInformation("  - Suplidores únicos: {Count}", suplidores.Count);

        // 3. Fechas únicas (yyyyMMdd)
        _logger.LogInformation("Paso 3: Generando dimensión de tiempo...");
        var fechas = rowsList
            .Select(r => r.Fecha.Date)
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

        // 4. Cargar dimensiones primero
        _logger.LogInformation("Paso 4: Cargando dimensiones en DWH...");
        await _dwhRepository.LoadDataAsync(
            categorias, productos, clientes, suplidores, fechas, Enumerable.Empty<FactVentas>(),
            cancellationToken);
        _logger.LogInformation("Paso 4 completado. Dimensiones guardadas.");

        // 5. Resolver FKs y crear hechos
        _logger.LogInformation("Paso 5: Resolviendo claves foráneas y construyendo hechos...");
        var productoKeys = await _dwhRepository.GetProductoKeysAsync(productos.Select(p => p.Codigo), cancellationToken);
        var clienteKeys = await _dwhRepository.GetClienteKeysAsync(clientes.Select(c => c.ClienteIdOrigen), cancellationToken);
        var categoriaKeys = await _dwhRepository.GetCategoriaKeysAsync(categorias.Select(c => c.NombreCategoria), cancellationToken);
        var suplidorKeys = await _dwhRepository.GetSuplidorKeysAsync(suplidores.Select(s => s.SuplidorIdOrigen), cancellationToken);

        var hechos = rowsList.Select(r => new FactVentas
        {
            ProductoKey = productoKeys[r.Producto],
            ClienteKey = clienteKeys[r.Cliente],
            FechaKey = int.Parse(r.Fecha.ToString("yyyyMMdd")),
            Cantidad = r.Cantidad,
            PrecioUnitario = r.PrecioBase,
            TotalVenta = r.Total
        }).ToList();
        _logger.LogInformation("  - Hechos construidos: {Count}", hechos.Count);

        // 6. Guardar hechos
        _logger.LogInformation("Paso 6: Guardando hechos en DWH...");
        await _dwhRepository.LoadDataAsync(
            Enumerable.Empty<DimCategoria>(),
            Enumerable.Empty<DimProducto>(),
            Enumerable.Empty<DimCliente>(),
            Enumerable.Empty<DimSuplidor>(),
            Enumerable.Empty<DimFecha>(),
            hechos,
            cancellationToken);

        _logger.LogInformation("=== PROCESO ETL COMPLETADO EXITOSAMENTE ===");
        _logger.LogInformation("Resumen: {Categorias} categorías, {Productos} productos, {Clientes} clientes, {Suplidores} suplidores, {Fechas} fechas, {Hechos} hechos",
            categorias.Count, productos.Count, clientes.Count, suplidores.Count, fechas.Count, hechos.Count);
    }
}