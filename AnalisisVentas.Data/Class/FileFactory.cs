using AnalisisVentas.Data.Interfaces;

namespace AnalisisVentas.Data.Class;


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
