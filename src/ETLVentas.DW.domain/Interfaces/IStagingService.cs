namespace ETLVentas.DW.domain.Interfaces;

// Contrato para persistir y re-leer los datos extraídos en archivos temporales (staging).
// La Fase 1 (Extracción) escribe; la Fase 2 (Carga) lee los mismos archivos para alimentar el DWH.
public interface IStagingService
{
    Task WriteAsync<T>(string name, IEnumerable<T> records, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ReadAsync<T>(string name, CancellationToken cancellationToken = default);
}
