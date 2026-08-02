namespace AnalisisVentas.Data.Interfaces;

// Principio I (Interface Segregation): interfaz específica para fuentes API.
// Principio D (Dependency Inversion): el Worker depende de esta abstracción.
public interface IApiReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromApiAsync(string url);
}
