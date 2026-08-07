using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ETLVentas.DW.domain.Entities.Dimensions;

namespace ETLVentas.DW.persistencia.Configurations;

public class DimClienteConfiguration : IEntityTypeConfiguration<DimCliente>
{
    public void Configure(EntityTypeBuilder<DimCliente> builder)
    {
        builder.ToTable("DimCliente", "Dimensiones");
        builder.HasKey(c => c.ClienteKey);
        builder.Property(c => c.ClienteIdOrigen).HasMaxLength(50).IsRequired();
        builder.Property(c => c.NombreCompleto).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.Telefono).HasMaxLength(50);
        builder.Property(c => c.Ciudad).HasMaxLength(100);
        builder.HasIndex(c => c.ClienteIdOrigen).IsUnique();
    }
}