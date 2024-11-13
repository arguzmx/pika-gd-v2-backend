using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion;
using System.Diagnostics.CodeAnalysis;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionUsuarioDominio : IEntityTypeConfiguration<UsuarioDominio>
{
    public void Configure(EntityTypeBuilder<UsuarioDominio> builder)
    {
        builder.ToTable(DbContextOrganizacion.TABLA_USUARIO_DOMINIO);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UsuarioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
    }
}
