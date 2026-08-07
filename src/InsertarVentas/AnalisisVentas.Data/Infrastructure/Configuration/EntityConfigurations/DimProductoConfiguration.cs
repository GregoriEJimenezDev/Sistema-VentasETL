using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnalisisVentas.Data.Domain.Entities.Dimensions;

namespace AnalisisVentas.Data.Infrastructure.Configuration.EntityConfigurations;

public class DimProductoConfiguration : IEntityTypeConfiguration<DimProducto>
{
    public void Configure(EntityTypeBuilder<DimProducto> builder)
    {
        builder.ToTable("DimProducto", "Dimensiones");
        builder.HasKey(p => p.ProductoKey);
        builder.Property(p => p.Codigo).HasMaxLength(50).IsRequired();
        builder.Property(p => p.NombreProducto).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Categoria).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Precio).HasColumnType("decimal(18,2)");
        builder.HasIndex(p => p.Codigo).IsUnique();
    }
}