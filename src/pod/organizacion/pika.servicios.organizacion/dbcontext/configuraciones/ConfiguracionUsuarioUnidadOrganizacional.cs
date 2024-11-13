using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion;
using System.Diagnostics.CodeAnalysis;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionUsuarioUnidadOrganizacional : IEntityTypeConfiguration<UsuarioUnidadOrganizacional>
{

    public void Configure(EntityTypeBuilder<UsuarioUnidadOrganizacional> builder)
    {
        builder.ToTable(DbContextOrganizacion.TABLA_USUARIO_UNIDADES_ORG);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UsuarioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.DominioId).IsRequired();
        builder.Property(e => e.UnidadOrganizacionalId).IsRequired().HasMaxLength(128);
    }
}
