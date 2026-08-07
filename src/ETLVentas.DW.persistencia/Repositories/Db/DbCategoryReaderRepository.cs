using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.persistencia.Repositories.Db;


public class DbCategoryReaderRepository : IDbReaderRepository<Category>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbCategoryReaderRepository> _logger;

    public DbCategoryReaderRepository(IConfiguration configuration, ILogger<DbCategoryReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> ReadFromDbAsync()
    {
        var categorias = new List<Category>();
        try
        {
            _logger.LogInformation("Iniciando lectura de Categories desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT CategoryID, CategoryName FROM dbo.Categories";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categorias.Add(new Category
                {
                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                });
            }

            _logger.LogInformation("Lectura de Categories completada — {Cantidad} categorías", categorias.Count);
            return categorias;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer Categories desde la base de datos");
            throw;
        }
    }
}
