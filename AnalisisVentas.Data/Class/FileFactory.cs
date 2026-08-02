using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Class;

// Principio S: esta clase solo se encarga de delegar la lectura de un archivo
// conociendo su ruta, sin conocer la implementación del lector.
// Principio O: abierta para nuevos tipos de archivo sin modificar esta clase.
// Principio D: depende de la abstracción IFileReaderRepository<TModel>.
public class FileFactory<TModel> where TModel : class
{
    // Ruta del archivo inyectada por constructor, nunca hardcodeada.
    private readonly string _filePath;

    public FileFactory(string filePath)
    {
        _filePath = filePath;
    }

    // Recibe cualquier implementación de IFileReaderRepository por parámetro
    // y delega la lectura, desacoplando ruta de la implementación concreta.
    public Task<IEnumerable<TModel>> ReadData(IFileReaderRepository<TModel> fileReader)
    {
        return fileReader.ReadFileAsync(_filePath);
    }
}
