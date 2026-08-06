using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Domain.Entities.Facts;


[Table("FactVentas", Schema = "Hechos")]
public class FactVentas
{
    [Key]
    public int VentaKey { get; set; }
    public int ProductoKey { get; set; }
    public int ClienteKey { get; set; }
    public int FechaKey { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal TotalVenta { get; set; }
}