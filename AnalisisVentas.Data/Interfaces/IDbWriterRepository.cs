namespace AnalisisVentas.Data.Interfaces;

// Contrato para insertar/actualizar cualquier entidad en el DWH.

public interface IDbWriterRepository<TClass> where TClass : class
{
    // Realiza un UPSERT (SCD Tipo 1) y retorna el Key generado o existente.
    Task<int> UpsertAsync(TClass entity);
}
