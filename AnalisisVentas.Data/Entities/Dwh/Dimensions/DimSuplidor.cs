using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Entities.Dwh.Dimensions;

// Principio S (Single Responsibility): POCO puro, solo representa la tabla DimSuplidor.
[Table("DimSuplidor", Schema = "Dimensiones")]
public class DimSuplidor
{
    [Key]
    public int SuplidorKey { get; set; }
    public int SuplidorIdOrigen { get; set; }
    public string NombreSuplidor { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public DateTime FechaCreacionDW { get; set; }
}
