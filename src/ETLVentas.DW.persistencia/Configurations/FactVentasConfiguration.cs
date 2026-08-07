using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ETLVentas.DW.domain.Entities.Facts;

namespace ETLVentas.DW.persistencia.Configurations;

public class FactVentasConfiguration : IEntityTypeConfiguration<FactVentas>
{
    public void Configure(EntityTypeBuilder<FactVentas> builder)
    {
        builder.ToTable("FactVentas", "Hechos");
        builder.HasKey(f => f.VentaKey);
        builder.Property(f => f.PrecioUnitario).HasColumnType("decimal(18,2)");
        builder.Property(f => f.TotalVenta).HasColumnType("decimal(18,2)");
        builder.HasIndex(f => new { f.ProductoKey, f.ClienteKey, f.FechaKey }).IsUnique();
    }
}