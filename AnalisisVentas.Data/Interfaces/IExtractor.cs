namespace AnalisisVentas.Data.Interfaces;


public interface IExtractor<T>
{
    Task<IEnumerable<T>> ExtractAsync(CancellationToken cancellationToken = default);
}
