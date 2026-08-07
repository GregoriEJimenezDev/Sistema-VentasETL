using ETLVentas.DW.domain.Entities.Dimensions;
using ETLVentas.DW.domain.Entities.Facts;

namespace ETLVentas.DW.domain.Interfaces;

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
    Task<Dictionary<string, int>> GetCategoriaKeysAsync(IEnumerable<string> nombres, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetSuplidorKeysAsync(IEnumerable<string> idsOrigen, CancellationToken cancellationToken = default);
}