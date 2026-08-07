using ETLVentas.DW.domain.Entities.Api;
using Microsoft.AspNetCore.Mvc;

namespace ETLVentas.DW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuplidoresController : ControllerBase
{
    private static readonly List<Supplier> _suplidores = new()
    {
        new Supplier
        {
            Id = 1,
            Email = "juan.perez@proveedor.com",
            Username = "juanperez",
            Phone = "+51-987-654-321",
            Name = new SupplierName { Firstname = "Juan", Lastname = "Perez" },
            Address = new SupplierAddress { City = "Lima", Street = "Av. Industrial 123", Zipcode = "15001" }
        },
        new Supplier
        {
            Id = 2,
            Email = "maria.garcia@distribuidora.com",
            Username = "mariagarcia",
            Phone = "+51-956-789-012",
            Name = new SupplierName { Firstname = "Maria", Lastname = "Garcia" },
            Address = new SupplierAddress { City = "Arequipa", Street = "Calle Comercio 456", Zipcode = "04001" }
        },
        new Supplier
        {
            Id = 3,
            Email = "carlos.lopez@suministros.com",
            Username = "carloslopez",
            Phone = "+51-945-123-789",
            Name = new SupplierName { Firstname = "Carlos", Lastname = "Lopez" },
            Address = new SupplierAddress { City = "Trujillo", Street = "Jr. Manufactura 789", Zipcode = "13001" }
        },
        new Supplier
        {
            Id = 4,
            Email = "ana.martinez@logistica.com",
            Username = "anamartinez",
            Phone = "+51-934-567-890",
            Name = new SupplierName { Firstname = "Ana", Lastname = "Martinez" },
            Address = new SupplierAddress { City = "Chiclayo", Street = "Av. Almacenes 321", Zipcode = "14001" }
        },
        new Supplier
        {
            Id = 5,
            Email = "pedro.sanchez@importaciones.com",
            Username = "pedrosanchez",
            Phone = "+51-923-456-789",
            Name = new SupplierName { Firstname = "Pedro", Lastname = "Sanchez" },
            Address = new SupplierAddress { City = "Piura", Street = "Calle Puerto 654", Zipcode = "20001" }
        }
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_suplidores);
    }
}
