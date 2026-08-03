namespace AnalisisVentas.Data.Entities.Db;


public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryID { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
