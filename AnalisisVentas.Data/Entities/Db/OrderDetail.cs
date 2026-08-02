namespace AnalisisVentas.Data.Entities.Db;

// Principio S (Single Responsibility): POCO puro, solo representa la entidad Order_Details.
public class OrderDetail
{
    public int DetailID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
