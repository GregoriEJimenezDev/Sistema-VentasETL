namespace ETLVentas.DW.domain.Interfaces;


public interface IApiReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromApiAsync(string url);
}
