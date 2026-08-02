using AnalisisVentas.Data.Entities.Dwh.Facts;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Dwh;

// Principio S: esta clase solo inserta filas en Hechos.FactVentas sin duplicar.
// Principio D: depende de abstracciones (IConfiguration, ILogger).
// Si el hecho ya existe (misma combinación de keys) se retorna el key sin insertar.
public class FactVentasWriterRepository : IDbWriterRepository<FactVentas>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FactVentasWriterRepository> _logger;

    public FactVentasWriterRepository(IConfiguration configuration, ILogger<FactVentasWriterRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<int> UpsertAsync(FactVentas entity)
    {
        try
        {
            _logger.LogInformation(
                "Iniciando UPSERT en FactVentas — ProductoKey: {ProductoKey}, ClienteKey: {ClienteKey}, FechaKey: {FechaKey}",
                entity.ProductoKey, entity.ClienteKey, entity.FechaKey);

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            const string selectQuery = @"
                SELECT VentaKey FROM Hechos.FactVentas
                WHERE ProductoKey = @ProductoKey AND ClienteKey = @ClienteKey AND FechaKey = @FechaKey";
            var ventaKey = 0;

            await using (var selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@ProductoKey", entity.ProductoKey);
                selectCommand.Parameters.AddWithValue("@ClienteKey", entity.ClienteKey);
                selectCommand.Parameters.AddWithValue("@FechaKey", entity.FechaKey);

                var result = await selectCommand.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    ventaKey = Convert.ToInt32(result);
                    _logger.LogInformation("FactVentas ya existía, no se duplica — VentaKey: {VentaKey}", ventaKey);
                    return ventaKey;
                }
            }

            const string insertQuery = @"
                INSERT INTO Hechos.FactVentas (ProductoKey, ClienteKey, FechaKey, Cantidad, PrecioUnitario, TotalVenta)
                VALUES (@ProductoKey, @ClienteKey, @FechaKey, @Cantidad, @PrecioUnitario, @TotalVenta);
                SELECT SCOPE_IDENTITY();";

            await using var insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@ProductoKey", entity.ProductoKey);
            insertCommand.Parameters.AddWithValue("@ClienteKey", entity.ClienteKey);
            insertCommand.Parameters.AddWithValue("@FechaKey", entity.FechaKey);
            insertCommand.Parameters.AddWithValue("@Cantidad", entity.Cantidad);
            insertCommand.Parameters.AddWithValue("@PrecioUnitario", entity.PrecioUnitario);
            insertCommand.Parameters.AddWithValue("@TotalVenta", entity.TotalVenta);

            var identity = await insertCommand.ExecuteScalarAsync();
            ventaKey = Convert.ToInt32(identity);

            _logger.LogInformation("FactVentas insertado — VentaKey: {VentaKey}", ventaKey);
            return ventaKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en UPSERT de FactVentas");
            throw;
        }
    }
}
