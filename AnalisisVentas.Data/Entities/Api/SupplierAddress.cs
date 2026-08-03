using System.Text.Json.Serialization;

namespace AnalisisVentas.Data.Entities.Api;


public class SupplierAddress
{
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("street")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("zipcode")]
    public string Zipcode { get; set; } = string.Empty;
}
