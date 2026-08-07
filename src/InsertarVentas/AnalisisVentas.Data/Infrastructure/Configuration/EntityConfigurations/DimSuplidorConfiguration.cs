using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnalisisVentas.Data.Domain.Entities.Dimensions;

namespace AnalisisVentas.Data.Infrastructure.Configuration.EntityConfigurations;

public class DimSuplidorConfiguration : IEntityTypeConfiguration<DimSuplidor>
{
    public void Configure(EntityTypeBuilder<DimSuplidor> builder)
    {
        builder.ToTable("DimSuplidor", "Dimensiones");
        builder.HasKey(s => s.SuplidorKey);
        builder.Property(s => s.SuplidorIdOrigen).HasMaxLength(50).IsRequired();
        builder.Property(s => s.NombreSuplidor).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Email).HasMaxLength(150);
        builder.Property(s => s.Telefono).HasMaxLength(50);
        builder.Property(s => s.Ciudad).HasMaxLength(100);
        builder.HasIndex(s => s.SuplidorIdOrigen).IsUnique();
    }
}