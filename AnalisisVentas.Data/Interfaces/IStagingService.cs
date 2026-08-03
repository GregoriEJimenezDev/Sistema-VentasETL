namespace AnalisisVentas.Data.Interfaces;

// Contrato para persistir los datos extraídos en archivos temporales (staging),
// cumpliendo el requisito de la fase de extracción del ETL.
public interface IStagingService
{
    Task WriteAsync<T>(string nombre, IEnumerable<T> registros, CancellationToken cancellationToken = default);
}
