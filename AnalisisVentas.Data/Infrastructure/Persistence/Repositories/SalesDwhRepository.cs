using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AnalisisVentas.Data.Domain.Entities.Dimensions;
using AnalisisVentas.Data.Domain.Entities.Facts;
using AnalisisVentas.Data.Domain.Interfaces;
using AnalisisVentas.Data.Infrastructure.Persistence;

namespace AnalisisVentas.Data.Infrastructure.Persistence.Repositories;

public class SalesDwhRepository : ISalesDwhRepository
{
    private readonly VentasDwhContext _context;
    private readonly ILogger<SalesDwhRepository> _logger;

    public SalesDwhRepository(VentasDwhContext context, ILogger<SalesDwhRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LoadDataAsync(
        IEnumerable<DimCategoria> categorias,
        IEnumerable<DimProducto> productos,
        IEnumerable<DimCliente> clientes,
        IEnumerable<DimSuplidor> suplidores,
        IEnumerable<DimFecha> fechas,
        IEnumerable<FactVentas> hechos,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Iniciando carga de datos en DWH");

        // Crear tablas si no existen
        await _context.Database.EnsureCreatedAsync(cancellationToken);
        _logger.LogInformation("Base de datos verificada/creada");

        // 1. Categorías - UPSERT
        _logger.LogInformation("Procesando DimCategoria: {Count} registros", categorias.Count());
        var existingCategorias = await _context.DimCategorias
            .ToDictionaryAsync(c => c.NombreCategoria, cancellationToken);
        
        foreach (var cat in categorias)
        {
            if (existingCategorias.TryGetValue(cat.NombreCategoria, out var existing))
            {
                _context.Entry(existing).CurrentValues.SetValues(cat);
            }
            else
            {
                await _context.DimCategorias.AddAsync(cat, cancellationToken);
            }
        }

        // 2. Productos - UPSERT
        _logger.LogInformation("Procesando DimProducto: {Count} registros", productos.Count());
        var existingProductos = await _context.DimProductos
            .ToDictionaryAsync(p => p.Codigo, cancellationToken);
        
        foreach (var prod in productos)
        {
            if (existingProductos.TryGetValue(prod.Codigo, out var existing))
            {
                _context.Entry(existing).CurrentValues.SetValues(prod);
            }
            else
            {
                await _context.DimProductos.AddAsync(prod, cancellationToken);
            }
        }

        // 3. Clientes - UPSERT
        _logger.LogInformation("Procesando DimCliente: {Count} registros", clientes.Count());
        var existingClientes = await _context.DimClientes
            .ToDictionaryAsync(c => c.ClienteIdOrigen, cancellationToken);
        
        foreach (var cli in clientes)
        {
            if (existingClientes.TryGetValue(cli.ClienteIdOrigen, out var existing))
            {
                _context.Entry(existing).CurrentValues.SetValues(cli);
            }
            else
            {
                await _context.DimClientes.AddAsync(cli, cancellationToken);
            }
        }

        // 4. Suplidores - UPSERT
        _logger.LogInformation("Procesando DimSuplidor: {Count} registros", suplidores.Count());
        var existingSuplidores = await _context.DimSuplidores
            .ToDictionaryAsync(s => s.SuplidorIdOrigen, cancellationToken);
        
        foreach (var supl in suplidores)
        {
            if (existingSuplidores.TryGetValue(supl.SuplidorIdOrigen, out var existing))
            {
                _context.Entry(existing).CurrentValues.SetValues(supl);
            }
            else
            {
                await _context.DimSuplidores.AddAsync(supl, cancellationToken);
            }
        }

        // 5. Fechas - Solo insertar las que no existen
        _logger.LogInformation("Procesando DimFecha: {Count} registros", fechas.Count());
        var existingFechas = await _context.DimFechas
            .Select(f => f.FechaKey)
            .ToListAsync(cancellationToken);
        var existingFechasSet = new HashSet<int>(existingFechas);
        
        var nuevasFechas = fechas.Where(f => !existingFechasSet.Contains(f.FechaKey));
        await _context.DimFechas.AddRangeAsync(nuevasFechas, cancellationToken);

        // 6. Hechos - Anti-duplicado por clave compuesta
        _logger.LogInformation("Procesando FactVentas: {Count} registros", hechos.Count());
        var existingHechos = await _context.FactVentas
            .Select(f => new { f.ProductoKey, f.ClienteKey, f.FechaKey })
            .ToListAsync(cancellationToken);
        var existingHechosSet = new HashSet<(int ProductoKey, int ClienteKey, int FechaKey)>(
            existingHechos.Select(h => (h.ProductoKey, h.ClienteKey, h.FechaKey)));
        
        var nuevosHechos = hechos.Where(f => !existingHechosSet.Contains(
            (f.ProductoKey, f.ClienteKey, f.FechaKey)));
        
        await _context.FactVentas.AddRangeAsync(nuevosHechos, cancellationToken);

        // Guardar todo en una sola transacción
        _logger.LogInformation("Guardando cambios en base de datos...");
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Carga de datos completada exitosamente");
    }

    public async Task<Dictionary<string, int>> GetProductoKeysAsync(IEnumerable<string> codigos, CancellationToken cancellationToken = default)
    {
        return await _context.DimProductos
            .Where(p => codigos.Contains(p.Codigo))
            .ToDictionaryAsync(p => p.Codigo, p => p.ProductoKey, cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetClienteKeysAsync(IEnumerable<string> idsOrigen, CancellationToken cancellationToken = default)
    {
        return await _context.DimClientes
            .Where(c => idsOrigen.Contains(c.ClienteIdOrigen))
            .ToDictionaryAsync(c => c.ClienteIdOrigen, c => c.ClienteKey, cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetCategoriaKeysAsync(IEnumerable<string> nombres, CancellationToken cancellationToken = default)
    {
        return await _context.DimCategorias
            .Where(c => nombres.Contains(c.NombreCategoria))
            .ToDictionaryAsync(c => c.NombreCategoria, c => c.CategoriaKey, cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetSuplidorKeysAsync(IEnumerable<string> idsOrigen, CancellationToken cancellationToken = default)
    {
        return await _context.DimSuplidores
            .Where(s => idsOrigen.Contains(s.SuplidorIdOrigen))
            .ToDictionaryAsync(s => s.SuplidorIdOrigen, s => s.SuplidorKey, cancellationToken);
    }
}