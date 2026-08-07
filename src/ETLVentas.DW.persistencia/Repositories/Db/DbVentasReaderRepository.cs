using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.persistencia.Repositories.Db;

public class DbVentasReaderRepository : IDbReaderRepository<OrderDetail>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbVentasReaderRepository> _logger;

    public DbVentasReaderRepository(IConfiguration configuration, ILogger<DbVentasReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<OrderDetail>> ReadFromDbAsync()
    {
        var detalles = new List<OrderDetail>();
        try
        {
            _logger.LogInformation("Iniciando lectura de ventas desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT od.DetailID, od.OrderID, od.ProductID, od.Quantity, od.UnitPrice, od.TotalPrice FROM dbo.Order_Details od";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                detalles.Add(new OrderDetail
                {
                    DetailID = reader.GetInt32(reader.GetOrdinal("DetailID")),
                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))
                });
            }

            _logger.LogInformation("Lectura de ventas desde la base de datos completada — {Cantidad} detalles", detalles.Count);
            return detalles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer ventas desde la base de datos");
            throw;
        }
    }
}
