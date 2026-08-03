using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Entities.Dwh.Dimensions;


[Table("DimProducto", Schema = "Dimensiones")]
public class DimProducto
{
    [Key]
    public int ProductoKey { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaCreacionDW { get; set; }
}
