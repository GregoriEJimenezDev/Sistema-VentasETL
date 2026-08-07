using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETLVentas.DW.persistencia.Repositories.Db;


public class DbCityReaderRepository : IDbReaderRepository<City>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbCityReaderRepository> _logger;

    public DbCityReaderRepository(IConfiguration configuration, ILogger<DbCityReaderRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<City>> ReadFromDbAsync()
    {
        var ciudades = new List<City>();
        try
        {
            _logger.LogInformation("Iniciando lectura de Cities desde la base de datos");

            var connectionString = _configuration.GetConnectionString("SistemaVentasETL");
            const string query = "SELECT CityID, CityName, CountryID FROM dbo.Cities";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ciudades.Add(new City
                {
                    CityID = reader.GetInt32(reader.GetOrdinal("CityID")),
                    CityName = reader.GetString(reader.GetOrdinal("CityName")),
                    CountryID = reader.GetInt32(reader.GetOrdinal("CountryID"))
                });
            }

            _logger.LogInformation("Lectura de Cities completada — {Cantidad} ciudades", ciudades.Count);
            return ciudades;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer Cities desde la base de datos");
            throw;
        }
    }
}
