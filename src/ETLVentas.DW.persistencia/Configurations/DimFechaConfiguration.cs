using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ETLVentas.DW.domain.Entities.Dimensions;

namespace ETLVentas.DW.persistencia.Configurations;

public class DimFechaConfiguration : IEntityTypeConfiguration<DimFecha>
{
    public void Configure(EntityTypeBuilder<DimFecha> builder)
    {
        builder.ToTable("DimFecha", "Dimensiones");
        builder.HasKey(f => f.FechaKey);
        builder.Property(f => f.FechaKey).ValueGeneratedNever();
        builder.Property(f => f.Fecha).HasColumnType("date").IsRequired();
        builder.Property(f => f.NombreMes).HasMaxLength(20).IsRequired();
        builder.Property(f => f.DiaNombre).HasMaxLength(20).IsRequired();
        builder.HasIndex(f => f.FechaKey).IsUnique();
    }
}