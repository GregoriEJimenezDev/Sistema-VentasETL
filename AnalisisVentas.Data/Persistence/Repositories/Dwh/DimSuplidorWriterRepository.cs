using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Dwh;


// Estrategia SCD Tipo 1: si el SuplidorIdOrigen existe se actualiza, si no se inserta.
public class DimSuplidorWriterRepository : IDbWriterRepository<DimSuplidor>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DimSuplidorWriterRepository> _logger;

    public DimSuplidorWriterRepository(IConfiguration configuration, ILogger<DimSuplidorWriterRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<int> UpsertAsync(DimSuplidor entity)
    {
        try
        {
            _logger.LogInformation("Iniciando UPSERT en DimSuplidor — SuplidorIdOrigen: {SuplidorIdOrigen}", entity.SuplidorIdOrigen);

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            const string selectQuery = "SELECT SuplidorKey FROM Dimensiones.DimSuplidor WHERE SuplidorIdOrigen = @SuplidorIdOrigen";
            var suplidorKey = 0;

            await using (var selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@SuplidorIdOrigen", entity.SuplidorIdOrigen);
                var result = await selectCommand.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    suplidorKey = Convert.ToInt32(result);

                    const string updateQuery = @"
                        UPDATE Dimensiones.DimSuplidor
                        SET NombreSuplidor = @NombreSuplidor,
                            Email = @Email,
                            Telefono = @Telefono,
                            Ciudad = @Ciudad
                        WHERE SuplidorKey = @SuplidorKey";

                    await using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NombreSuplidor", entity.NombreSuplidor);
                    updateCommand.Parameters.AddWithValue("@Email", entity.Email);
                    updateCommand.Parameters.AddWithValue("@Telefono", entity.Telefono);
                    updateCommand.Parameters.AddWithValue("@Ciudad", entity.Ciudad);
                    updateCommand.Parameters.AddWithValue("@SuplidorKey", suplidorKey);
                    await updateCommand.ExecuteNonQueryAsync();

                    _logger.LogInformation("DimSuplidor actualizado — SuplidorKey: {SuplidorKey}", suplidorKey);
                    return suplidorKey;
                }
            }

            const string insertQuery = @"
                INSERT INTO Dimensiones.DimSuplidor (SuplidorIdOrigen, NombreSuplidor, Email, Telefono, Ciudad)
                VALUES (@SuplidorIdOrigen, @NombreSuplidor, @Email, @Telefono, @Ciudad);
                SELECT SCOPE_IDENTITY();";

            await using var insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@SuplidorIdOrigen", entity.SuplidorIdOrigen);
            insertCommand.Parameters.AddWithValue("@NombreSuplidor", entity.NombreSuplidor);
            insertCommand.Parameters.AddWithValue("@Email", entity.Email);
            insertCommand.Parameters.AddWithValue("@Telefono", entity.Telefono);
            insertCommand.Parameters.AddWithValue("@Ciudad", entity.Ciudad);

            var identity = await insertCommand.ExecuteScalarAsync();
            suplidorKey = Convert.ToInt32(identity);

            _logger.LogInformation("DimSuplidor insertado — SuplidorKey: {SuplidorKey}", suplidorKey);
            return suplidorKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en UPSERT de DimSuplidor — SuplidorIdOrigen: {SuplidorIdOrigen}", entity.SuplidorIdOrigen);
            throw;
        }
    }
}
