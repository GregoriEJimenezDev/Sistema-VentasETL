using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AnalisisVentas.Api.Controllers;

// Principio S: el controlador solo expone suplidores obtenidos de la API.
// Principio D: depende de la abstracción IApiReaderRepository<Supplier>, no de la concreción.
[ApiController]
[Route("api/[controller]")]
public class SuplidoresController : ControllerBase
{
    private readonly IApiReaderRepository<Supplier> _suplidorRepo;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuplidoresController> _logger;

    public SuplidoresController(
        IApiReaderRepository<Supplier> suplidorRepo,
        IConfiguration configuration,
        ILogger<SuplidoresController> logger)
    {
        _suplidorRepo = suplidorRepo;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            _logger.LogInformation("Solicitando suplidores desde el controlador");

            var url = _configuration["ApiSettings:SuppliersUrl"]!;
            var suplidores = await _suplidorRepo.ReadFromApiAsync(url);

            return Ok(suplidores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener suplidores en el controlador");
            return Problem("Ocurrió un error al obtener los suplidores.");
        }
    }
}
