using CsvHelper.Configuration.Attributes;

namespace AnalisisVentas.Data.Entities.Csv;


public class ProductoCsv
{
    [Name("ProductoId")]
    public int ProductoId { get; set; }

    [Name("Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Name("Categoria")]
    public string Categoria { get; set; } = string.Empty;

    [Name("Precio")]
    public decimal Precio { get; set; }

    [Name("Stock")]
    public int Stock { get; set; }
}
