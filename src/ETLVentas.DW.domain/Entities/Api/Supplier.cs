using System.Text.Json.Serialization;

namespace ETLVentas.DW.domain.Entities.Api;


public class Supplier
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public SupplierName Name { get; set; } = new();

    [JsonPropertyName("address")]
    public SupplierAddress Address { get; set; } = new();
}
