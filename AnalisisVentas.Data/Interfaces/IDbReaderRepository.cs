namespace AnalisisVentas.Data.Interfaces;


public interface IDbReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromDbAsync();
}
