namespace AnalisisVentas.Data.Entities.Db;

// Principio S (Single Responsibility): POCO puro, solo representa la entidad Customers.
public class Customer
{
    public int CustomerID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int CityID { get; set; }
}
