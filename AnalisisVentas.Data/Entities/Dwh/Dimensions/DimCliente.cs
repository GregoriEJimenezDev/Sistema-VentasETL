using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Entities.Dwh.Dimensions;

// Principio S (Single Responsibility): POCO puro, solo representa la tabla DimCliente.
[Table("DimCliente", Schema = "Dimensiones")]
public class DimCliente
{
    [Key]
    public int ClienteKey { get; set; }
    public string ClienteIdOrigen { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public DateTime FechaCreacionDW { get; set; }
}
