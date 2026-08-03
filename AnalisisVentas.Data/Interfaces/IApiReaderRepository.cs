namespace AnalisisVentas.Data.Interfaces;


public interface IApiReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromApiAsync(string url);
}
