using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Entities.Dwh.Dimensions;


[Table("DimSuplidor", Schema = "Dimensiones")]
public class DimSuplidor
{
    [Key]
    public int SuplidorKey { get; set; }
    public string SuplidorIdOrigen { get; set; } = string.Empty;
    public string NombreSuplidor { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public DateTime FechaCreacionDW { get; set; }
}
