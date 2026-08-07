namespace ETLVentas.DW.domain.Interfaces;


public interface IDbReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromDbAsync();
}
