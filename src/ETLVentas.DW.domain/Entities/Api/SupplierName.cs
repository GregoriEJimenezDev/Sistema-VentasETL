using System.Text.Json.Serialization;

namespace ETLVentas.DW.domain.Entities.Api;


public class SupplierName
{
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string Lastname { get; set; } = string.Empty;
}
