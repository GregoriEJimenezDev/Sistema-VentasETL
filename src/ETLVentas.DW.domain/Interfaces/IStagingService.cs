namespace ETLVentas.DW.domain.Interfaces;

// Contrato para persistir los datos extraídos en archivos temporales (staging),
public interface IStagingService
{
    Task WriteAsync<T>(string name, IEnumerable<T> records, CancellationToken cancellationToken = default);
}
