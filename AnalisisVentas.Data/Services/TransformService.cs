using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Entities.Dwh.Facts;

namespace AnalisisVentas.Data.Services;

// Principio S: esta clase estática solo se encarga de transformar/mapear entidades
// de origen (BD, CSV, API) hacia las entidades del DWH. Sin estado y sin I/O.
public static class TransformService
{
    // Mapea Product + Category → DimProducto.
    public static DimProducto MapProductoToDim(Product product, Category category)
    {
        return new DimProducto
        {
            Codigo = product.ProductID.ToString(),
            NombreProducto = product.ProductName,
            Categoria = category.CategoryName,
            Precio = product.Price,
            Stock = product.Stock,
            FechaCreacionDW = DateTime.Now
        };
    }

    // Mapea Customer + ciudad → DimCliente.
    public static DimCliente MapClienteToDim(Customer customer, string ciudad)
    {
        return new DimCliente
        {
            ClienteIdOrigen = customer.CustomerID.ToString(),
            NombreCompleto = $"{customer.FirstName} {customer.LastName}".Trim(),
            Email = customer.Email,
            Telefono = customer.Phone,
            Ciudad = ciudad,
            FechaCreacionDW = DateTime.Now
        };
    }

    // Mapea Supplier (API) → DimSuplidor.
    public static DimSuplidor MapSuplidorToDim(Supplier supplier)
    {
        return new DimSuplidor
        {
            SuplidorIdOrigen = supplier.Id.ToString(),
            NombreSuplidor = $"{supplier.Name.Firstname} {supplier.Name.Lastname}".Trim(),
            Email = supplier.Email,
            Telefono = supplier.Phone,
            Ciudad = supplier.Address.City,
            FechaCreacionDW = DateTime.Now
        };
    }

    // Mapea Order + OrderDetail → FactVentas usando los keys ya resueltos.
    // FechaKey se construye como yyyyMMdd para coincidir con Dimensiones.DimFecha.
    public static FactVentas MapToFactVentas(Order order, OrderDetail detail, int productoKey, int clienteKey)
    {
        return new FactVentas
        {
            ProductoKey = productoKey,
            ClienteKey = clienteKey,
            FechaKey = int.Parse(order.OrderDate.ToString("yyyyMMdd")),
            Cantidad = detail.Quantity,
            PrecioUnitario = detail.UnitPrice,
            TotalVenta = detail.TotalPrice
        };
    }
}
