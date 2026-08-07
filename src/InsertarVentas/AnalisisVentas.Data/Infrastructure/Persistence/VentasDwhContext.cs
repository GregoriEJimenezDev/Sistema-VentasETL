using Microsoft.EntityFrameworkCore;
using AnalisisVentas.Data.Domain.Entities.Dimensions;
using AnalisisVentas.Data.Domain.Entities.Facts;
using AnalisisVentas.Data.Infrastructure.Configuration.EntityConfigurations;

namespace AnalisisVentas.Data.Infrastructure.Persistence;

public class VentasDwhContext : DbContext
{
    public VentasDwhContext(DbContextOptions<VentasDwhContext> options) : base(options) { }

    public DbSet<DimCategoria> DimCategorias => Set<DimCategoria>();
    public DbSet<DimProducto> DimProductos => Set<DimProducto>();
    public DbSet<DimCliente> DimClientes => Set<DimCliente>();
    public DbSet<DimSuplidor> DimSuplidores => Set<DimSuplidor>();
    public DbSet<DimFecha> DimFechas => Set<DimFecha>();
    public DbSet<FactVentas> FactVentas => Set<FactVentas>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DimCategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new DimProductoConfiguration());
        modelBuilder.ApplyConfiguration(new DimClienteConfiguration());
        modelBuilder.ApplyConfiguration(new DimSuplidorConfiguration());
        modelBuilder.ApplyConfiguration(new DimFechaConfiguration());
        modelBuilder.ApplyConfiguration(new FactVentasConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}