namespace AnalisisVentas.Data.Entities.Db;

// Principio S (Single Responsibility): POCO puro, solo representa la entidad Cities.
public class City
{
    public int CityID { get; set; }
    public string CityName { get; set; } = string.Empty;
    public int CountryID { get; set; }
}
