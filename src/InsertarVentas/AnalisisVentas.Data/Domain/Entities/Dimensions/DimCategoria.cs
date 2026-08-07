using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Domain.Entities.Dimensions;


[Table("DimCategoria", Schema = "Dimensiones")]
public class DimCategoria
{
    [Key]
    public int CategoriaKey { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public DateTime FechaCreacionDW { get; set; }
}