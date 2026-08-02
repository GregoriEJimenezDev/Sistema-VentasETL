namespace AnalisisVentas.Data.Interfaces;

// Principio I (Interface Segregation): interfaz específica para fuentes de base de datos.
// Principio D (Dependency Inversion): el Worker depende de esta abstracción.
public interface IDbReaderRepository<TClass> where TClass : class
{
    Task<IEnumerable<TClass>> ReadFromDbAsync();
}
