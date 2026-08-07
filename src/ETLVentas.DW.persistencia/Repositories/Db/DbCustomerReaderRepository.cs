using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.persistencia.Repositories.Db;


public class DbCustomerReaderRepository : IDbReaderRepository<Customer>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbCustomerReaderRepository> _logger;

    public DbCustomerReaderRepository(IConfiguration configuration, ILogger<DbCustomerReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>> ReadFromDbAsync()
    {
        var clientes = new List<Customer>();
        try
        {
            _logger.LogInformation("Iniciando lectura de Customers desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT CustomerID, FirstName, LastName, Email, Phone, CityID FROM dbo.Customers";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientes.Add(new Customer
                {
                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("Phone")),
                    CityID = reader.GetInt32(reader.GetOrdinal("CityID"))
                });
            }

            _logger.LogInformation("Lectura de Customers completada — {Cantidad} clientes", clientes.Count);
            return clientes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer Customers desde la base de datos");
            throw;
        }
    }
}
