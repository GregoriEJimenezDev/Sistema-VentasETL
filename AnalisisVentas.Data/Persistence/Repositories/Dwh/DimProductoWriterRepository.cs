using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Dwh;

// Principio S: esta clase solo hace UPSERT sobre Dimensiones.DimProducto.
// Principio D: depende de abstracciones (IConfiguration, ILogger).
// Estrategia SCD Tipo 1: si el Codigo existe se actualiza, si no se inserta.
public class DimProductoWriterRepository : IDbWriterRepository<DimProducto>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DimProductoWriterRepository> _logger;

    public DimProductoWriterRepository(IConfiguration configuration, ILogger<DimProductoWriterRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<int> UpsertAsync(DimProducto entity)
    {
        try
        {
            _logger.LogInformation("Iniciando UPSERT en DimProducto — Codigo: {Codigo}", entity.Codigo);

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            const string selectQuery = "SELECT ProductoKey FROM Dimensiones.DimProducto WHERE Codigo = @Codigo";
            var productoKey = 0;

            await using (var selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@Codigo", entity.Codigo);
                var result = await selectCommand.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    productoKey = Convert.ToInt32(result);

                    const string updateQuery = @"
                        UPDATE Dimensiones.DimProducto
                        SET NombreProducto = @NombreProducto,
                            Categoria = @Categoria,
                            Precio = @Precio,
                            Stock = @Stock
                        WHERE ProductoKey = @ProductoKey";

                    await using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NombreProducto", entity.NombreProducto);
                    updateCommand.Parameters.AddWithValue("@Categoria", entity.Categoria);
                    updateCommand.Parameters.AddWithValue("@Precio", entity.Precio);
                    updateCommand.Parameters.AddWithValue("@Stock", entity.Stock);
                    updateCommand.Parameters.AddWithValue("@ProductoKey", productoKey);
                    await updateCommand.ExecuteNonQueryAsync();

                    _logger.LogInformation("DimProducto actualizado — ProductoKey: {ProductoKey}", productoKey);
                    return productoKey;
                }
            }

            const string insertQuery = @"
                INSERT INTO Dimensiones.DimProducto (Codigo, NombreProducto, Categoria, Precio, Stock)
                VALUES (@Codigo, @NombreProducto, @Categoria, @Precio, @Stock);
                SELECT SCOPE_IDENTITY();";

            await using var insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@Codigo", entity.Codigo);
            insertCommand.Parameters.AddWithValue("@NombreProducto", entity.NombreProducto);
            insertCommand.Parameters.AddWithValue("@Categoria", entity.Categoria);
            insertCommand.Parameters.AddWithValue("@Precio", entity.Precio);
            insertCommand.Parameters.AddWithValue("@Stock", entity.Stock);

            var identity = await insertCommand.ExecuteScalarAsync();
            productoKey = Convert.ToInt32(identity);

            _logger.LogInformation("DimProducto insertado — ProductoKey: {ProductoKey}", productoKey);
            return productoKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en UPSERT de DimProducto — Codigo: {Codigo}", entity.Codigo);
            throw;
        }
    }
}
