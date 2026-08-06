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
        _logger.LogInformation("Iniciando proceso ETL desde CSV: {FilePath}", csvFilePath);

        // 1. Leer CSV
        var rows = await _csvReader.ReadAsync(csvFilePath, cancellationToken);
        var rowsList = rows.ToList();
        _logger.LogInformation("Leídos {Count} registros del CSV", rowsList.Count);

        // 2. Transformar y deduplicar dimensiones con LINQ
        var categorias = rowsList
            .Select(r => r.CategoriaNombre)
            .Distinct()
            .Select(n => new DimCategoria
            {
                NombreCategoria = n,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();

        var productos = rowsList
            .GroupBy(r => r.ProductoCodigo)
            .Select(g => g.First())
            .Select(r => new DimProducto
            {
                Codigo = r.ProductoCodigo,
                NombreProducto = r.ProductoNombre,
                Categoria = r.CategoriaNombre,
                Precio = r.Precio,
                Stock = r.Stock,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();

        var clientes = rowsList
            .GroupBy(r => r.ClienteId)
            .Select(g => g.First())
            .Select(r => new DimCliente
            {
                ClienteIdOrigen = r.ClienteId,
                NombreCompleto = r.ClienteNombre,
                Email = r.ClienteEmail,
                Telefono = r.ClienteTelefono,
                Ciudad = r.ClienteCiudad,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();

        var suplidores = rowsList
            .GroupBy(r => r.SuplidorId)
            .Select(g => g.First())
            .Select(r => new DimSuplidor
            {
                SuplidorIdOrigen = r.SuplidorId,
                NombreSuplidor = r.SuplidorNombre,
                Email = r.SuplidorEmail,
                Telefono = r.SuplidorTelefono,
                Ciudad = r.SuplidorCiudad,
                FechaCreacionDW = DateTime.UtcNow
            })
            .ToList();

        // 3. Fechas únicas (yyyyMMdd)
        var fechas = rowsList
            .Select(r => r.FechaVenta.Date)
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

        // 4. Cargar dimensiones primero
        await _dwhRepository.LoadDataAsync(
            categorias, productos, clientes, suplidores, fechas, Enumerable.Empty<FactVentas>(),
            cancellationToken);

        // 5. Resolver FKs y crear hechos
        var productoKeys = await _dwhRepository.GetProductoKeysAsync(productos.Select(p => p.Codigo), cancellationToken);
        var clienteKeys = await _dwhRepository.GetClienteKeysAsync(clientes.Select(c => c.ClienteIdOrigen), cancellationToken);

        var hechos = rowsList.Select(r => new FactVentas
        {
            ProductoKey = productoKeys[r.ProductoCodigo],
            ClienteKey = clienteKeys[r.ClienteId],
            FechaKey = int.Parse(r.FechaVenta.ToString("yyyyMMdd")),
            Cantidad = r.Cantidad,
            PrecioUnitario = r.PrecioUnitario,
            TotalVenta = r.TotalVenta
        }).ToList();

        // 6. Guardar hechos
        await _dwhRepository.LoadDataAsync(
            Enumerable.Empty<DimCategoria>(),
            Enumerable.Empty<DimProducto>(),
            Enumerable.Empty<DimCliente>(),
            Enumerable.Empty<DimSuplidor>(),
            Enumerable.Empty<DimFecha>(),
            hechos,
            cancellationToken);

        _logger.LogInformation("Proceso ETL completado. Hechos insertados: {Count}", hechos.Count);
    }
}