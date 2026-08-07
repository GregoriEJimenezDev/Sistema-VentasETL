using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.persistencia.Repositories.Db;


public class DbProductReaderRepository : IDbReaderRepository<Product>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbProductReaderRepository> _logger;

    public DbProductReaderRepository(IConfiguration configuration, ILogger<DbProductReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> ReadFromDbAsync()
    {
        var productos = new List<Product>();
        try
        {
            _logger.LogInformation("Iniciando lectura de Products desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT ProductID, ProductName, CategoryID, Price, Stock FROM dbo.Products";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                productos.Add(new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                });
            }

            _logger.LogInformation("Lectura de Products completada — {Cantidad} productos", productos.Count);
            return productos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer Products desde la base de datos");
            throw;
        }
    }
}
