namespace AnalisisVentas.Data.Interfaces;

// Principio D: abstracción que unifica la extracción sin importar la fuente
// (CSV, BD relacional o API REST). Permite agregar nuevas fuentes sin modificar
// el orquestador (Principio O: abierta para extensión).
public interface IExtractor<T>
{
    Task<IEnumerable<T>> ExtractAsync(CancellationToken cancellationToken = default);
}
