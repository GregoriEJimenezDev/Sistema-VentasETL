namespace AnalisisVentas.Data.Interfaces;

// Contrato para insertar/actualizar cualquier entidad en el DWH.
// Principio I (Interface Segregation): interfaz específica para escritura.
// Principio D (Dependency Inversion): el Worker depende de esta abstracción.
public interface IDbWriterRepository<TClass> where TClass : class
{
    // Realiza un UPSERT (SCD Tipo 1) y retorna el Key generado o existente.
    Task<int> UpsertAsync(TClass entity);
}
