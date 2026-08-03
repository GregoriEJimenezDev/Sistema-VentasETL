using System.Text.Json.Serialization;

namespace AnalisisVentas.Data.Entities.Api;


public class SupplierName
{
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string Lastname { get; set; } = string.Empty;
}
