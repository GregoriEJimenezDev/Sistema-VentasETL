using AnalisisVentas.Data.Domain.Entities.Dimensions;
using AnalisisVentas.Data.Domain.Entities.Facts;

namespace AnalisisVentas.Data.Domain.Interfaces;

public interface ICsvVentasFileReaderRepository
{
    Task<IEnumerable<VentaCsvRow>> ReadAsync(string filePath, CancellationToken cancellationToken = default);
}

public record VentaCsvRow(
    string ProductoCodigo,
    string ProductoNombre,
    string CategoriaNombre,
    decimal Precio,
    int Stock,
    string ClienteId,
    string ClienteNombre,
    string ClienteEmail,
    string ClienteTelefono,
    string ClienteCiudad,
    string SuplidorId,
    string SuplidorNombre,
    string SuplidorEmail,
    string SuplidorTelefono,
    string SuplidorCiudad,
    DateTime FechaVenta,
    int Cantidad,
    decimal PrecioUnitario,
    decimal TotalVenta
);