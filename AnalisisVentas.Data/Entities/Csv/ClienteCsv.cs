using CsvHelper.Configuration.Attributes;

namespace AnalisisVentas.Data.Entities.Csv;

public class ClienteCsv
{
    [Name("ClienteId")]
    public int ClienteId { get; set; }

    [Name("Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Name("Email")]
    public string Email { get; set; } = string.Empty;

    [Name("Region")]
    public string Region { get; set; } = string.Empty;
}
