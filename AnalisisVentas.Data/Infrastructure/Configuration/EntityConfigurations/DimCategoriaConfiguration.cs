using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnalisisVentas.Data.Domain.Entities.Dimensions;

namespace AnalisisVentas.Data.Infrastructure.Configuration.EntityConfigurations;

public class DimCategoriaConfiguration : IEntityTypeConfiguration<DimCategoria>
{
    public void Configure(EntityTypeBuilder<DimCategoria> builder)
    {
        builder.ToTable("DimCategoria", "Dimensiones");
        builder.HasKey(c => c.CategoriaKey);
        builder.Property(c => c.NombreCategoria).HasMaxLength(100).IsRequired();
        builder.HasIndex(c => c.NombreCategoria).IsUnique();
    }
}