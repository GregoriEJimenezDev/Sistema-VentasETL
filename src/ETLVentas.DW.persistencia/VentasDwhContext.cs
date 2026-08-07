using Microsoft.EntityFrameworkCore;
using ETLVentas.DW.domain.Entities.Dimensions;
using ETLVentas.DW.domain.Entities.Facts;
using ETLVentas.DW.persistencia.Configurations;

namespace ETLVentas.DW.persistencia;

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