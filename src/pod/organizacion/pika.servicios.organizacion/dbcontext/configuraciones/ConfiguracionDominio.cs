using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion;
using System.Diagnostics.CodeAnalysis;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionDominio : IEntityTypeConfiguration<Dominio>
{
    public void Configure(EntityTypeBuilder<Dominio> builder)
    {
        builder.ToTable(DbContextOrganizacion.TABLA_DOMINIO);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Activo).IsRequired();
        builder.Property(e => e.FechaCreacion).IsRequired();
        builder.Property(e => e.UsuarioId).IsRequired().HasMaxLength(128);

        // Al eliminar un dominio todos sus elementos relacionados se eliminan en cascada
        builder.HasMany(x => x.UnidadesOrganizacionales).WithOne(y => y.Dominio).HasForeignKey(z => z.DominioId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.UsuarioDominio).WithOne(y => y.Dominio).HasForeignKey(z => z.DominioId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.UsuarioUnidadOrganizacionals).WithOne(y => y.Dominio).HasForeignKey(z => z.DominioId).OnDelete(DeleteBehavior.Cascade);
    }
}