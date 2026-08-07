using System.Globalization;

namespace ETLVentas.DW.domain.Interfaces;

public interface ICsvVentasFileReaderRepository
{
    Task<IEnumerable<VentaCsvRow>> ReadAsync(string filePath, CancellationToken cancellationToken = default);
}

public record VentaCsvRow(
    DateTime Fecha,
    string Categoria,
    string Producto,
    string Cliente,
    string Suplidor,
    int Cantidad,
    decimal PrecioBase,
    decimal Total
);