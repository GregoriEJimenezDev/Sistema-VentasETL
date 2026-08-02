namespace AnalisisVentas.Data.Interfaces;

// Principio I (Interface Segregation): interfaz específica para fuentes de archivo.
// Principio D (Dependency Inversion): el Worker depende de esta abstracción.
public interface IFileReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFileAsync(string filePath);
}
