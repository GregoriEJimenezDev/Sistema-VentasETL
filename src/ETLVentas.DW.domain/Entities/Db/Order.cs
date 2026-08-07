namespace ETLVentas.DW.domain.Entities.Db;


public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public int StatusID { get; set; }
    public DateTime OrderDate { get; set; }
}
