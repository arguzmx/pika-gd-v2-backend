using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion;
using System.Diagnostics.CodeAnalysis;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionUnidadOrganizacional : IEntityTypeConfiguration<UnidadOrganizacional>
{
    public void Configure(EntityTypeBuilder<UnidadOrganizacional> builder)
    {
        builder.ToTable(DbContextOrganizacion.TABLA_UNIDADES_ORG);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(128);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);

        // Cuando una OU desaparece sus vinculos con los usuarios tambien de manera automatica con el Cascade
        builder.HasMany(x => x.UsuariosUnidad).WithOne(y => y.UnidadOrganizacional).HasForeignKey(z => z.UnidadOrganizacionalId).OnDelete(DeleteBehavior.Cascade);     
    }
}
