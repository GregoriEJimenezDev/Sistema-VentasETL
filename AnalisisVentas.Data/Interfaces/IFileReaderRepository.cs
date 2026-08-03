namespace AnalisisVentas.Data.Interfaces;


public interface IFileReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFileAsync(string filePath);
}
