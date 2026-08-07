namespace ETLVentas.DW.domain.Interfaces;


public interface IExtractor<T>
{
    Task<IEnumerable<T>> ExtractAsync(CancellationToken cancellationToken = default);
}
