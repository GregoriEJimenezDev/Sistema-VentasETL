namespace AnalisisVentas.Data.Entities.Db;

// Principio S (Single Responsibility): POCO puro, solo representa la entidad Orders.
public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public int StatusID { get; set; }
    public DateTime OrderDate { get; set; }
}
