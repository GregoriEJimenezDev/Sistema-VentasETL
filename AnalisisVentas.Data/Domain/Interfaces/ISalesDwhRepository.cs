using AnalisisVentas.Data.Domain.Entities.Dimensions;
using AnalisisVentas.Data.Domain.Entities.Facts;

namespace AnalisisVentas.Data.Domain.Interfaces;

public interface ISalesDwhRepository
{
    Task LoadDataAsync(
        IEnumerable<DimCategoria> categorias,
        IEnumerable<DimProducto> productos,
        IEnumerable<DimCliente> clientes,
        IEnumerable<DimSuplidor> suplidores,
        IEnumerable<DimFecha> fechas,
        IEnumerable<FactVentas> hechos,
        CancellationToken cancellationToken = default);

    Task<Dictionary<string, int>> GetProductoKeysAsync(IEnumerable<string> codigos, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetClienteKeysAsync(IEnumerable<string> idsOrigen, CancellationToken cancellationToken = default);
}