using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Data.Persistence.Repositories.Db;

// Principio S: esta clase solo lee la tabla dbo.Orders.
// Principio D: depende de abstracciones (IConfiguration, ILogger).
public class DbOrderReaderRepository : IDbReaderRepository<Order>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbOrderReaderRepository> _logger;

    public DbOrderReaderRepository(IConfiguration configuration, ILogger<DbOrderReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> ReadFromDbAsync()
    {
        var ordenes = new List<Order>();
        try
        {
            _logger.LogInformation("Iniciando lectura de Orders desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT OrderID, CustomerID, StatusID, OrderDate FROM dbo.Orders";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ordenes.Add(new Order
                {
                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    StatusID = reader.GetInt32(reader.GetOrdinal("StatusID")),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                });
            }

            _logger.LogInformation("Lectura de Orders completada — {Cantidad} órdenes", ordenes.Count);
            return ordenes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer Orders desde la base de datos");
            throw;
        }
    }
}
