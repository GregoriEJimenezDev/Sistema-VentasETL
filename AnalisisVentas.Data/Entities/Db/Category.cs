namespace AnalisisVentas.Data.Entities.Db;

// Principio S (Single Responsibility): POCO puro, solo representa la entidad Categories.
public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
