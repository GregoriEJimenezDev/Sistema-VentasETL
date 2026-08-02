using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Dwh;

// Principio S: esta clase solo hace UPSERT sobre Dimensiones.DimCliente.
// Principio D: depende de abstracciones (IConfiguration, ILogger).
// Estrategia SCD Tipo 1: si el ClienteIdOrigen existe se actualiza, si no se inserta.
public class DimClienteWriterRepository : IDbWriterRepository<DimCliente>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DimClienteWriterRepository> _logger;

    public DimClienteWriterRepository(IConfiguration configuration, ILogger<DimClienteWriterRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<int> UpsertAsync(DimCliente entity)
    {
        try
        {
            _logger.LogInformation("Iniciando UPSERT en DimCliente — ClienteIdOrigen: {ClienteIdOrigen}", entity.ClienteIdOrigen);

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            const string selectQuery = "SELECT ClienteKey FROM Dimensiones.DimCliente WHERE ClienteIdOrigen = @ClienteIdOrigen";
            var clienteKey = 0;

            await using (var selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@ClienteIdOrigen", entity.ClienteIdOrigen);
                var result = await selectCommand.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    clienteKey = Convert.ToInt32(result);

                    const string updateQuery = @"
                        UPDATE Dimensiones.DimCliente
                        SET NombreCompleto = @NombreCompleto,
                            Email = @Email,
                            Telefono = @Telefono,
                            Ciudad = @Ciudad
                        WHERE ClienteKey = @ClienteKey";

                    await using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NombreCompleto", entity.NombreCompleto);
                    updateCommand.Parameters.AddWithValue("@Email", entity.Email);
                    updateCommand.Parameters.AddWithValue("@Telefono", entity.Telefono);
                    updateCommand.Parameters.AddWithValue("@Ciudad", entity.Ciudad);
                    updateCommand.Parameters.AddWithValue("@ClienteKey", clienteKey);
                    await updateCommand.ExecuteNonQueryAsync();

                    _logger.LogInformation("DimCliente actualizado — ClienteKey: {ClienteKey}", clienteKey);
                    return clienteKey;
                }
            }

            const string insertQuery = @"
                INSERT INTO Dimensiones.DimCliente (ClienteIdOrigen, NombreCompleto, Email, Telefono, Ciudad)
                VALUES (@ClienteIdOrigen, @NombreCompleto, @Email, @Telefono, @Ciudad);
                SELECT SCOPE_IDENTITY();";

            await using var insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@ClienteIdOrigen", entity.ClienteIdOrigen);
            insertCommand.Parameters.AddWithValue("@NombreCompleto", entity.NombreCompleto);
            insertCommand.Parameters.AddWithValue("@Email", entity.Email);
            insertCommand.Parameters.AddWithValue("@Telefono", entity.Telefono);
            insertCommand.Parameters.AddWithValue("@Ciudad", entity.Ciudad);

            var identity = await insertCommand.ExecuteScalarAsync();
            clienteKey = Convert.ToInt32(identity);

            _logger.LogInformation("DimCliente insertado — ClienteKey: {ClienteKey}", clienteKey);
            return clienteKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en UPSERT de DimCliente — ClienteIdOrigen: {ClienteIdOrigen}", entity.ClienteIdOrigen);
            throw;
        }
    }
}
