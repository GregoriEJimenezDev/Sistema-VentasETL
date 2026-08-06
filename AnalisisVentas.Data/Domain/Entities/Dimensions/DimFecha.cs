using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Data.Domain.Entities.Dimensions;


[Table("DimFecha", Schema = "Dimensiones")]
public class DimFecha
{
    [Key]
    public int FechaKey { get; set; }
    public DateTime Fecha { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public int Dia { get; set; }
    public string NombreMes { get; set; } = string.Empty;
    public int Trimestre { get; set; }
    public int Semana { get; set; }
    public string DiaNombre { get; set; } = string.Empty;
    public bool EsFinSemana { get; set; }
    public DateTime FechaCreacionDW { get; set; }
}